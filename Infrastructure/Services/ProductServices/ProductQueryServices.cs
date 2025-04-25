using System.Linq.Expressions;
using Application.Abstractions.LinkService;
using Application.DTOs.Product;
using Application.Features.Pages;
using Application.IServices;
using Domain.Database.PostgreSQL.ReadModels;
using Domain.Entities.Products;
using Domain.Entities.ProductVariants;
using Domain.ReadModels;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL;

namespace Infrastructure.Services.ProductServices;

internal sealed class ProductQueryServices : IProductQueryServices
{
    private readonly SeeSharpPostgreSQLReadDbContext _postgreSQLdbContext;
    public ProductQueryServices(
        SeeSharpPostgreSQLReadDbContext postgreSQLdbContext)
    {
        _postgreSQLdbContext = postgreSQLdbContext;
    }

    public async Task<bool> CheckProductAvailability(ProductId productId)
    {
        return await _postgreSQLdbContext.Products.AnyAsync(
            x => x.ProductId == new Ulid(productId.Value) && x.ProductStatus == ProductStatus.InStock);
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

        return await _postgreSQLdbContext.Products.AnyAsync(x => x.ProductName == productName.Value);
    }

    public async Task<ProductVariantResponse?> GetVariantById(
        ProductVariantId productVariantId, 
        CancellationToken cancellationToken = default)
    {
        return await _postgreSQLdbContext.ProductVariants
            .Where(x => x.ProductVariantId == new Ulid(productVariantId.Value) && x.ProductVariantStatus != ProductVariantStatus.Discontinued)
            .Select(a => new ProductVariantResponse(
                a.ProductVariantId.ToGuid(),
                a.ProductId.ToGuid(),
                a.ProductReadModel!.ProductName,
                a.VariantName,
                a.ColorCode,
                a.Description,
                a.ProductReadModel!.CategoryReadModel.CategoryName,
                a.ProductVariantPrice,
                a.ImageUrl ?? string.Empty,
                a.IsBaseVariant))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ProductVariantResponse?> GetVariantByIdForAdmin(
        ProductVariantId productVariantId, 
        CancellationToken cancellationToken = default)
    {
        return await _postgreSQLdbContext.ProductVariants
            .Where(x => x.ProductVariantId == new Ulid(productVariantId.Value))
            .Select(a => new ProductVariantResponse(
                a.ProductVariantId.ToGuid(),
                a.ProductId.ToGuid(),
                a.ProductReadModel!.ProductName,
                a.VariantName,
                a.ColorCode,
                a.Description,
                a.ProductReadModel!.CategoryReadModel.CategoryName,
                a.ProductVariantPrice,
                a.ImageUrl ?? string.Empty,
                a.IsBaseVariant))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsProductExist(ProductId productId)
    {
        return await _postgreSQLdbContext.Products
            .AnyAsync(x => x.ProductId == new Ulid(productId.Value) && x.ProductStatus != ProductStatus.Discontinued);
    }

    public async Task<bool> IsProductVariantNameExist(
        ProductId productId,
        ProductVariantId? productVariantId,
        VariantName productVariantName,
        CancellationToken cancellationToken = default)
    {
        if (productVariantId is not null)
        {
            return await _postgreSQLdbContext.ProductVariants
                .AnyAsync(
                x => x.VariantName == productVariantName.Value
                && x.ProductId == new Ulid(productId.Value)
                && x.ProductVariantId != new Ulid(productVariantId.Value));
        }

        return await _postgreSQLdbContext.ProductVariants
                .AnyAsync(
                x => x.VariantName == productVariantName.Value
                && x.ProductId == new Ulid(productId.Value));
    }

    public async Task<ProductResponse?> GetProductWithVariantListById(
        ProductId productId,
        CancellationToken cancellationToken = default)
    {
        #region Old Code
        return await _postgreSQLdbContext.Products
            .Where(a => a.ProductId == new Ulid(productId.Value))
            .Select(a => new ProductResponse(
                a.ProductId.ToGuid(),
                a.ProductVariantReadModels.FirstOrDefault(b => b.IsBaseVariant)!.ProductVariantId.ToGuid(),
                a.ProductName,
                a.ProductVariantReadModels.FirstOrDefault(b => b.IsBaseVariant)!.ProductVariantPrice,
                a.ImageUrl,
                a.Description,
                a.ProductStatus.ToString(),
                a.CategoryReadModel.CategoryName,
                a.ProductVariantReadModels.Select(b => new VariantResponse(
                    b.ProductVariantId.ToGuid(),
                    b.ProductId.ToGuid(),
                    b.VariantName,
                    b.ColorCode,
                    b.Description,
                    b.ProductVariantPrice,
                    b.ProductVariantStatus.ToString(),
                    b.ImageUrl ?? string.Empty,
                    b.IsBaseVariant)).ToArray())).FirstOrDefaultAsync();
        #endregion
    }


    public async Task<PagedList<ProductResponse>> GetAllProductWithVariantList(
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

        //Filter by ProductStatus
        if (!string.IsNullOrWhiteSpace(filterProductStatus))
        {
            productsQuery = productsQuery.Where(x => x.ProductStatus == (ProductStatus)Enum.Parse(typeof(ProductStatus), filterProductStatus));
        }

        //Filter by CategoryId
        if (!string.IsNullOrWhiteSpace(filterCategory))
        {
            var id = new Guid(filterCategory);
            productsQuery = productsQuery.Where(x => x.CategoryId == new Ulid(id));
        }

        //sort
        Expression<Func<ProductReadModel, object>> keySelector = sortColumn?.ToLower() switch
        {
            "productname" => x => x.ProductName,
            "price" => x => x.ProductVariantReadModels.FirstOrDefault(b => b.IsBaseVariant)!.ProductVariantPrice,
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
            .Select(a => new ProductResponse(
                a.ProductId.ToGuid(),
                a.ProductVariantReadModels.FirstOrDefault(b => b.IsBaseVariant)!.ProductVariantId.ToGuid(),
                a.ProductName,
                a.ProductVariantReadModels.FirstOrDefault(b => b.IsBaseVariant)!.ProductVariantPrice,
                a.ImageUrl,
                a.Description,
                a.ProductStatus.ToString(),
                a.CategoryReadModel.CategoryName,
                a.ProductVariantReadModels.Select(b => new VariantResponse(
                    b.ProductVariantId.ToGuid(),
                    b.ProductId.ToGuid(),
                    b.VariantName,
                    b.ColorCode,
                    b.Description,
                    b.ProductVariantPrice,
                    b.ProductVariantStatus.ToString(),
                    b.ImageUrl ?? string.Empty,
                    b.IsBaseVariant)).ToArray())).AsQueryable();
            

        var productsList = await PagedList<ProductResponse>
            .CreateAsync(products, page ?? 1, pageSize ?? 10);

        return productsList;
    }

    public async Task<PagedList<ProductVariantResponse>> GetAllVariant(
        string? filterProductStatus,
        string? filterProduct,
        string? filterCategory, 
        string? searchTerm, 
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize)
    {
        var productVariantQuery = _postgreSQLdbContext.ProductVariants
            .Include(a => a.ProductReadModel)
            .ThenInclude(a => a!.CategoryReadModel)
            .AsQueryable();

        //Search
        if (!string.IsNullOrEmpty(searchTerm))
        {
            productVariantQuery = productVariantQuery.Where(x => x.ProductReadModel!.ProductName.Contains(searchTerm));
        }

        //Filter
        //Filter by ProductStatus
        if (!string.IsNullOrWhiteSpace(filterProductStatus))
        {
            productVariantQuery = productVariantQuery
                .Where(x => x.ProductVariantStatus == (ProductVariantStatus)Enum.Parse(typeof(ProductVariantStatus), 
                filterProductStatus));
        }

        //Filter by CategoryId
        if (!string.IsNullOrWhiteSpace(filterCategory) && Ulid.TryParse(filterCategory, out var _))
        {
            productVariantQuery = productVariantQuery.Where(
                x => x.ProductReadModel!.CategoryId == Ulid.Parse(filterCategory));
        }

        //sort
        Expression<Func<ProductVariantReadModel, object>> keySelector = sortColumn?.ToLower() switch
        {
            "productname" => x => x.ProductReadModel!.ProductName,
            "productid" => x => x.ProductId,
            "productvariantid" => x => x.ProductVariantId,
            "categoryid" => x => x.ProductReadModel!.CategoryReadModel.CategoryId,
            _ => x => x.ProductId
        };

        if (sortOrder?.ToLower() == "desc")
        {
            productVariantQuery = productVariantQuery.OrderByDescending(keySelector);
        }
        else
        {
            productVariantQuery = productVariantQuery.OrderBy(keySelector);
        }

        //paged
        var productVariants = productVariantQuery
            .Select(a => new ProductVariantResponse(
                a.ProductVariantId.ToGuid(),
                a.ProductId.ToGuid(),
                a.ProductReadModel!.ProductName,
                a.VariantName,
                a.ColorCode,
                a.Description,
                a.ProductReadModel!.CategoryReadModel.CategoryName,
                a.ProductVariantPrice,
                a.ImageUrl ?? string.Empty,
                a.IsBaseVariant)).AsQueryable();

        var productVariantList = await PagedList<ProductVariantResponse>
            .CreateAsync(productVariants, page ?? 1, pageSize ?? 10);
        
        return productVariantList;
    }

    public async Task<ProductVariantPrice?> GetAvailableProductPrice(ProductVariantId productVariantId) 
    {
        return await _postgreSQLdbContext.ProductVariants
            .Where(x => x.ProductVariantId == new Ulid(productVariantId.Value) 
            && x.ProductVariantStatus == ProductVariantStatus.InStock)
            .Select(x => ProductVariantPrice.FromDecimal(x.ProductVariantPrice))
            .FirstOrDefaultAsync();
    }

    public async Task<bool> IsProductVariantExist(ProductVariantId productVariantId)
    {
        return await _postgreSQLdbContext.ProductVariants
            .AnyAsync(x => x.ProductVariantId == new Ulid(productVariantId.Value) && x.ProductVariantStatus != ProductVariantStatus.Discontinued);
    }
}
