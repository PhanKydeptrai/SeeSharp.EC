using Application.DTOs.Product;
using Application.Features.Pages;
using Application.IServices;
using Domain.Entities.Products;
using Domain.Entities.ProductVariants;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Infrastructure.Services.ProductServices;

internal sealed class ProductQueryServicesDecorated : IProductQueryServices
{
    private readonly IProductQueryServices _decorated;
    private readonly IDistributedCache _distributedCache;
    public ProductQueryServicesDecorated(
        IProductQueryServices decorated,
        IDistributedCache distributedCache)
    {
        _decorated = decorated;
        _distributedCache = distributedCache;
    }

    public async Task<ProductResponse?> GetProductWithVariantListById(
        ProductId productId,
        CancellationToken cancellationToken = default)
    {
        string cacheKey = $"ProductResponse:{productId.Value}";
        string? cachedProduct = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);
        ProductResponse? product;
        if (string.IsNullOrEmpty(cachedProduct))
        {
            product = await _decorated.GetProductWithVariantListById(productId, cancellationToken);

            if (product is null) return product;

            await _distributedCache.SetStringAsync(
                cacheKey,
                JsonConvert.SerializeObject(product),
                cancellationToken);

            return product;
        }
        return JsonConvert.DeserializeObject<ProductResponse>(cachedProduct);
    }

    public async Task<bool> CheckProductAvailability(ProductId productId)
    {
        return await _decorated.CheckProductAvailability(productId);
    }

    public async Task<bool> IsProductNameExist(
        ProductId? productId, 
        ProductName productName, 
        CancellationToken cancellationToken = default)
    {
        return await _decorated.IsProductNameExist(productId, productName, cancellationToken);
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
        return await _decorated.GetAllProductWithVariantList(
            filterProductStatus, 
            filterCategory, 
            searchTerm, 
            sortColumn, 
            sortOrder, 
            page, 
            pageSize);
    }

    public async Task<bool> IsProductExist(ProductId productId)
    {
        return await _decorated.IsProductExist(productId);
    }

    public async Task<bool> IsProductVariantNameExist(
        ProductId productId,
        ProductVariantId? productVariantId,
        VariantName productVariantName,
        CancellationToken cancellationToken = default)
    {
        return await _decorated.IsProductVariantNameExist(productId, productVariantId, productVariantName);
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
        return await _decorated.GetAllVariant(
            filterProductStatus, 
            filterProduct, 
            filterCategory, 
            searchTerm, 
            sortColumn, 
            sortOrder, 
            page, 
            pageSize);
    }

    public async Task<ProductVariantPrice?> GetAvailableProductPrice(ProductVariantId productVariantId)
    {
        return await _decorated.GetAvailableProductPrice(productVariantId);
    }

    public async Task<ProductVariantResponse?> GetVariantById(ProductVariantId productVariantId, CancellationToken cancellationToken = default)
    {
        return await _decorated.GetVariantById(productVariantId, cancellationToken);
    }

    public async Task<bool> IsProductVariantExist(ProductVariantId productId)
    {
        return await _decorated.IsProductVariantExist(productId);
    }
}
