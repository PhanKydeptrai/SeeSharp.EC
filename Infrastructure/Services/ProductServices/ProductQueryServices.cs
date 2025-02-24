using Application.DTOs.Product;
using Application.Features.Pages;
using Application.IServices;
using Domain.Database.PostgreSQL.ReadModels;
using Domain.Entities.Categories;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL;
using System.Linq.Expressions;

namespace Infrastructure.Services.ProductServices;

internal sealed class ProductQueryServices : IProductQueryServices
{
    private readonly NextSharpPostgreSQLReadDbContext _postgreSQLdbContext;
    public ProductQueryServices(
        NextSharpPostgreSQLReadDbContext postgreSQLdbContext)
    {
        _postgreSQLdbContext = postgreSQLdbContext;
    }

    public async Task<ProductResponse?> GetById(
        ProductId productId,
        CancellationToken cancellationToken = default)
    {
        return await _postgreSQLdbContext.Products
            .Where(x => x.ProductId == new Ulid(productId.Value))
            .Select(x => new ProductResponse(
                x.ProductId.ToGuid(),
                x.ProductName,
                x.ImageUrl,
                x.Description,
                x.ProductPrice,
                x.ProductStatus,
                x.CategoryReadModel.CategoryName))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsProductNameExist(
        ProductId? productId, 
        ProductName productName, 
        CancellationToken cancellationToken = default)
    {
        if (productId is not null)
        {
            return await _postgreSQLdbContext.Products
                .AnyAsync(
                x => x.ProductName == productName.Value 
                && x.ProductId != new Ulid(productId.Value));
        }

        return await _postgreSQLdbContext.Products
            .Where(x => x.ProductName == productName.Value)
            .AnyAsync();
    }

    public async Task<PagedList<ProductResponse>> PagedList(
        string? filterProductStatus,
        string? filterCategory,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize)
    {
        var productsQuery = _postgreSQLdbContext.Products.AsQueryable();
        //Search
        if (!string.IsNullOrEmpty(searchTerm))
        {
            productsQuery = productsQuery.Where(
                x => x.ProductName.Contains(searchTerm));
        }

        //Filter
        if (!string.IsNullOrWhiteSpace(filterProductStatus))
        {
            productsQuery = productsQuery.Where(x => x.ProductStatus == filterProductStatus);
        }

        if (!string.IsNullOrWhiteSpace(filterCategory))
        {
            productsQuery = productsQuery.Where(
                x => x.CategoryId.ToGuid() == CategoryId.FromString(filterCategory));
        }

        //sort
        Expression<Func<ProductReadModel, object>> keySelector = sortColumn?.ToLower() switch
        {
            "productname" => x => x.ProductName,
            "productid" => x => x.ProductId,
            _ => x => x.ProductId
        };

        if (sortOrder?.ToLower() == "desc")
        {
            productsQuery = productsQuery.OrderByDescending(keySelector);
        }
        else
        {
            productsQuery = productsQuery.OrderBy(keySelector);
        }

        //paged
        var products = productsQuery
            .Select(x => new ProductResponse(
                x.ProductId.ToGuid(),
                x.ProductName,
                x.ImageUrl,
                x.Description,
                x.ProductPrice,
                x.ProductStatus,
                x.CategoryReadModel.CategoryName)).AsQueryable();
        var productsList = await PagedList<ProductResponse>
            .CreateAsync(products, page ?? 1, pageSize ?? 10);

        return productsList;
    }
}
