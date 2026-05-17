using Application.DTOs.Product;
using Application.Features.Pages;
using Application.IServices;
using Domain.Entities.Products;
using Domain.Entities.ProductVariants;
using Application.Helper;
using Newtonsoft.Json;
using Polly;
using Polly.Registry;
using SharedKernel.Constants;
using StackExchange.Redis;

namespace Infrastructure.Services.ProductServices;

internal sealed class ProductQueryServicesDecorated : IProductQueryServices
{
    private readonly IProductQueryServices _decorated;
    private readonly IRedisCacheService _cacheService;
    private readonly ICacheKeyGenerator _cacheKeyGenerator;

    private readonly TimeSpan _entityCacheTtl = TimeSpan.FromMinutes(30);

    public ProductQueryServicesDecorated(
        IProductQueryServices decorated,
        IRedisCacheService cacheService,
        ICacheKeyGenerator cacheKeyGenerator)
    {
        _decorated = decorated;
        _cacheService = cacheService;
        _cacheKeyGenerator = cacheKeyGenerator;
    }

    public async Task<ProductResponse?> GetProductWithVariantListById(
        ProductId productId,
        CancellationToken cancellationToken = default)
    {
        string cacheKey = $"ProductResponse:{productId.Value}";

        ProductResponse? cachedProduct = await _cacheService.GetAsync<ProductResponse>(cacheKey, cancellationToken);

        if (cachedProduct is not null)
        {
            return cachedProduct;
        }

        ProductResponse? product = await _decorated.GetProductWithVariantListById(productId, cancellationToken);

        if (product is not null)
        {
            await _cacheService.SetAsync(cacheKey, product, _entityCacheTtl, cancellationToken);
        }

        return product;
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

    private record CachedProductList(List<Guid> ProductIds, int Page, int PageSize, int TotalCount);
    private record CachedVariantList(List<Guid> VariantIds, int Page, int PageSize, int TotalCount);

    #region GetAllProductWithVariantList
    public async Task<PagedList<ProductResponse>> GetAllProductWithVariantList(
        string? filterProductStatus,
        string? filterCategory,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize)
    {

        // Tạo cache key dựa trên các tham số lọc, sắp xếp và phân trang
        string cacheKey = await _cacheKeyGenerator.CreateCacheKeyAsync(
            "ProductList",
            filterProductStatus,
            filterCategory,
            sortColumn,
            sortOrder,
            page,
            pageSize);

        // Lấy dữ liệu danh sách từ cache
        CachedProductList? listInfo = await _cacheService.GetAsync<CachedProductList>(cacheKey);

        if (listInfo is not null) //Nếu tìm thấy list id trong cache, tiếp tục lấy chi tiết từng sản phẩm
        {
            var redisKeys = listInfo.ProductIds.Select(id => $"ProductResponse:{id}").ToList();
            var cachedProducts = await _cacheService.GetManyAsync<ProductResponse>(redisKeys);

            var products = new List<ProductResponse>();
            var missingIds = new List<Guid>();

            for (int i = 0; i < listInfo.ProductIds.Count; i++)
            {
                if (cachedProducts[i] is not null)
                {
                    products.Add(cachedProducts[i]!);
                }
                else
                {
                    missingIds.Add(listInfo.ProductIds[i]);
                }
            }

            // Xử lý cache miss cho từng phần tử
            if (missingIds.Any())
            {
                var missingProducts = await _decorated.GetProductsByIds(
                    missingIds.Select(id => ProductId.FromGuid(id)),
                    CancellationToken.None);

                if (missingProducts.Any())
                {
                    var productsToCache = missingProducts.ToDictionary(
                        p => $"ProductResponse:{p.ProductId}",
                        p => p);

                    await _cacheService.SetManyAsync(productsToCache, _entityCacheTtl);
                    products.AddRange(missingProducts);
                }
            }

            return new PagedList<ProductResponse>(
                products,
                listInfo.Page,
                listInfo.PageSize,
                listInfo.TotalCount);
        }

        // Cache miss hoàn toàn, gọi service để lấy list id sản phẩm trong DB rồi cache lại
        var result = await _decorated.GetProductIdList(
            filterProductStatus,
            filterCategory,
            searchTerm,
            sortColumn,
            sortOrder,
            page,
            pageSize);

        // Nếu có kết quả thì cache lại thông tin list id sản phẩm
        if (result.ProductIds.Any())
        {
            var listToCache = new CachedProductList(
                result.ProductIds,
                result.Page,
                result.PageSize,
                result.TotalCount);

            await _cacheService.SetAsync(cacheKey, listToCache);

            // Query DB lấy dữ liệu chi tiết và cache lại
            var productList = await _decorated.GetProductsByIds(
                result.ProductIds.Select(id => ProductId.FromGuid(id)),
                CancellationToken.None);

            if (productList.Any())
            {
                var productsToCache = productList.ToDictionary(
                    p => $"ProductResponse:{p.ProductId}",
                    p => p);

                await _cacheService.SetManyAsync(productsToCache, _entityCacheTtl);
            }

            return new PagedList<ProductResponse>(
                productList,
                result.Page,
                result.PageSize,
                result.TotalCount);
        }

        return new PagedList<ProductResponse>(
            new List<ProductResponse>(),
            result.Page,
            result.PageSize,
            result.TotalCount);
    }
    #endregion

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
        string cacheKey = await _cacheKeyGenerator.CreateCacheKeyAsync("VariantList", filterProductStatus, filterCategory, sortColumn, sortOrder, page, pageSize, filterProduct);

        CachedVariantList? listInfo = await _cacheService.GetAsync<CachedVariantList>(cacheKey);

        if (listInfo is not null) // Nếu có cache, tiếp tục lấy chi tiết từng variant
        {
            var redisKeys = listInfo.VariantIds.Select(id => $"ProductVariantResponse:{id}").ToList();
            var cachedVariants = await _cacheService.GetManyAsync<ProductVariantResponse>(redisKeys);

            var variants = new List<ProductVariantResponse>();
            var missingIds = new List<Guid>();

            for (int i = 0; i < listInfo.VariantIds.Count; i++)
            {
                if (cachedVariants[i] is not null)
                {
                    variants.Add(cachedVariants[i]!);
                }
                else
                {
                    missingIds.Add(listInfo.VariantIds[i]);
                }
            }

            // Xử lý cache miss cho từng phần tử
            if (missingIds.Any())
            {
                var missingVariants = await _decorated.GetVariantsByIds(
                    missingIds.Select(id => ProductVariantId.FromGuid(id)),
                    CancellationToken.None);

                if (missingVariants.Any())
                {
                    var variantsToCache = missingVariants.ToDictionary(
                        v => $"ProductVariantResponse:{v.ProductVariantId}",
                        v => v);

                    await _cacheService.SetManyAsync(variantsToCache, _entityCacheTtl);
                    variants.AddRange(missingVariants);
                }
            }

            return new PagedList<ProductVariantResponse>(
                variants,
                listInfo.Page,
                listInfo.PageSize,
                listInfo.TotalCount);
        }

        // Cache miss hoàn toàn, gọi service để lấy list id variant trong DB rồi cache lại
        var result = await _decorated.GetVariantIdList(
            filterProductStatus,
            filterProduct,
            filterCategory,
            searchTerm,
            sortColumn,
            sortOrder,
            page,
            pageSize);

        // Nếu có kết quả thì cache lại thông tin list id variant
        if (result.VariantIds.Any())
        {
            var listToCache = new CachedVariantList(
                result.VariantIds,
                result.Page,
                result.PageSize,
                result.TotalCount);

            await _cacheService.SetAsync(cacheKey, listToCache);

            // Query DB lấy dữ liệu chi tiết và cache
            var variantList = await _decorated.GetVariantsByIds(
                result.VariantIds.Select(id => ProductVariantId.FromGuid(id)),
                CancellationToken.None);

            if (variantList.Any())
            {
                var variantsToCache = variantList.ToDictionary(
                    v => $"ProductVariantResponse:{v.ProductVariantId}",
                    v => v);

                await _cacheService.SetManyAsync(variantsToCache, _entityCacheTtl);
            }

            return new PagedList<ProductVariantResponse>(
                variantList,
                result.Page,
                result.PageSize,
                result.TotalCount);
        }

        return new PagedList<ProductVariantResponse>(
            new List<ProductVariantResponse>(),
            result.Page,
            result.PageSize,
            result.TotalCount);
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

    public async Task<List<ProductResponse>> GetProductsByIds(IEnumerable<ProductId> productIds, CancellationToken cancellationToken = default)
    {
        return await _decorated.GetProductsByIds(productIds, cancellationToken);
    }

    public async Task<List<ProductVariantResponse>> GetVariantsByIds(IEnumerable<ProductVariantId> variantIds, CancellationToken cancellationToken = default)
    {
        return await _decorated.GetVariantsByIds(variantIds, cancellationToken);
    }

    public Task<GetProductIdListResponse> GetProductIdList(
        string? filterProductStatus,
        string? filterCategory,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize)
    {
        return _decorated.GetProductIdList(
            filterProductStatus,
            filterCategory,
            searchTerm,
            sortColumn,
            sortOrder,
            page,
            pageSize);
    }

    public Task<GetVariantIdListResponse> GetVariantIdList(
        string? filterProductStatus,
        string? filterProduct,
        string? filterCategory,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize)
    {
        return _decorated.GetVariantIdList(
            filterProductStatus,
            filterProduct,
            filterCategory,
            searchTerm,
            sortColumn,
            sortOrder,
            page,
            pageSize);
    }
}
