using Application.Abstractions.LinkService;
using Application.DTOs.Product;
using Application.IServices;
using Domain.Entities.Products;
using Domain.Entities.ProductVariants;
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

    // public async Task<ProductResponse?> GetById(
    //     ProductId productId,
    //     CancellationToken cancellationToken = default)
    // {
    //     return await _postgreSQLdbContext.Products
    //         .Where(x => x.ProductId == new Ulid(productId.Value))
    //         .Select(x => new ProductResponse(
    //             x.ProductId.ToGuid(),
    //             x.ProductName,
    //             x.ImageUrl,
    //             x.Description,
    //             x.ProductPrice,
    //             x.ProductStatus.ToString(),
    //             x.CategoryReadModel.CategoryName))
    //         .FirstOrDefaultAsync(cancellationToken);
    // }

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

    public async Task<ProductResponse?> GetById(
        ProductId productId,
        CancellationToken cancellationToken = default)
    {
        return await _postgreSQLdbContext.Products
            .Where(a => a.ProductId == new Ulid(productId.Value))
            .Select(a => new ProductResponse(
                a.ProductId.ToGuid(),
                a.ProductName,
                a.ImageUrl,
                a.Description,
                a.ProductStatus.ToString(),
                a.CategoryReadModel.CategoryName,
                a.ProductVariantReadModels.Select(b => new VariantResponse(
                    b.ProductVariantId.ToGuid(),
                    b.VariantName,
                    b.ColorCode,
                    b.Description,
                    b.ProductVariantPrice,
                    b.ImageUrl ?? string.Empty,
                    b.IsBaseVariant)).ToArray()

            )).FirstOrDefaultAsync();
                                                 
    }


    // public async Task<PagedList<ProductResponse>> PagedList(
    //     string? filterProductStatus,
    //     string? filterCategory,
    //     string? searchTerm,
    //     string? sortColumn,
    //     string? sortOrder,
    //     int? page,
    //     int? pageSize)
    // {
    //     var productsQuery = _postgreSQLdbContext.Products.AsQueryable();
    //     //Search
    //     if (!string.IsNullOrEmpty(searchTerm))
    //     {
    //         productsQuery = productsQuery.Where(
    //             x => x.ProductName.Contains(searchTerm));
    //     }

    //     //Filter
    //     if (!string.IsNullOrWhiteSpace(filterProductStatus))
    //     {
    //         productsQuery = productsQuery.Where(x => x.ProductStatus == (ProductStatus)Enum.Parse(typeof(ProductStatus), filterProductStatus));
    //     }

    //     if (!string.IsNullOrWhiteSpace(filterCategory) && Ulid.TryParse(filterCategory, out var _))
    //     {
    //         productsQuery = productsQuery.Where(
    //             x => x.CategoryId == Ulid.Parse(filterCategory));
    //     }

    //     //sort
    //     Expression<Func<ProductReadModel, object>> keySelector = sortColumn?.ToLower() switch
    //     {
    //         "productname" => x => x.ProductName,
    //         "productid" => x => x.ProductId,
    //         "productprice" => x => x.ProductPrice,
    //         _ => x => x.ProductId
    //     };

    //     if (sortOrder?.ToLower() == "desc")
    //     {
    //         productsQuery = productsQuery.OrderByDescending(keySelector);
    //     }
    //     else
    //     {
    //         productsQuery = productsQuery.OrderBy(keySelector);
    //     }

    //     //paged
    //     var products = productsQuery
    //         .Select(x => new ProductResponse(
    //             x.ProductId.ToGuid(),
    //             x.ProductName,
    //             x.ImageUrl,
    //             x.Description,
    //             x.ProductPrice,
    //             x.ProductStatus.ToString(),
    //             x.CategoryReadModel.CategoryName)).AsQueryable();
    //     var productsList = await PagedList<ProductResponse>
    //         .CreateAsync(products, page ?? 1, pageSize ?? 10);

    //     return productsList;
    // }

    // public async Task<ProductVariantPrice?> GetAvailableProductPrice(ProductId productId) 
    // {
    //     return await _postgreSQLdbContext.Products
    //         .Where(x => x.ProductId == new Ulid(productId.Value) && x.ProductStatus == ProductStatus.InStock)
    //         .Select(x => ProductVariantPrice.FromDecimal(x.ProductPrice))
    //         .FirstOrDefaultAsync();
    // }
}
