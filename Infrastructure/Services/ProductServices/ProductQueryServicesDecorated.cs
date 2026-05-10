using System.Text;
using Application.DTOs.Product;
using Application.Features.Pages;
using Application.IServices;
using Domain.Entities.Products;
using Domain.Entities.ProductVariants;
using Infrastructure.Helper;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Polly;
using Polly.Registry;
using SharedKernel.Constants;
using StackExchange.Redis;

namespace Infrastructure.Services.ProductServices;

internal sealed class ProductQueryServicesDecorated : IProductQueryServices
{
    private readonly IProductQueryServices _decorated;
    private readonly IDistributedCache _distributedCache;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly ICacheKeyGenerator _cacheKeyGenerator;
    private readonly IAsyncPolicy<string?> _resiliencePolicy;

    private readonly TimeSpan _entityCacheTtl = TimeSpan.FromMinutes(30);

    public ProductQueryServicesDecorated(
        IProductQueryServices decorated,
        IDistributedCache distributedCache,
        IConnectionMultiplexer connectionMultiplexer,
        ICacheKeyGenerator cacheKeyGenerator,
        IReadOnlyPolicyRegistry<string> policyRegistry)
    {
        _decorated = decorated;
        _distributedCache = distributedCache;
        _connectionMultiplexer = connectionMultiplexer;
        _cacheKeyGenerator = cacheKeyGenerator;
        _resiliencePolicy = policyRegistry.Get<IAsyncPolicy<string?>>(Strategy.RedisStrategy);
    }

    public async Task<ProductResponse?> GetProductWithVariantListById(
        ProductId productId,
        CancellationToken cancellationToken = default)
    {
        string cacheKey = $"ProductResponse:{productId.Value}";
        string? cachedProduct = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);
        
        if (string.IsNullOrEmpty(cachedProduct))
        {
            ProductResponse? product = await _decorated.GetProductWithVariantListById(productId, cancellationToken);

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

    private record CachedProductList(List<Guid> ProductIds, int Page, int PageSize, int TotalCount);

    public async Task<PagedList<ProductResponse>> GetAllProductWithVariantList(
        string? filterProductStatus,
        string? filterCategory,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize)
    {
        string cacheKey = _cacheKeyGenerator.CreateCacheKey("ProductList", filterProductStatus, filterCategory, sortColumn, sortOrder, page, pageSize);

        string? cachedListInfo = await _resiliencePolicy.ExecuteAsync(
            async () =>
            {
                // Lấy dữ liệu danh sách từ cache
                string? cachedListInfo = await _distributedCache.GetStringAsync(cacheKey);
                return cachedListInfo;
            }
        );


        if (!string.IsNullOrEmpty(cachedListInfo)) // Nếu có cache, tiếp tục lấy chi tiết từng sản phẩm
        {
            var listInfo = JsonConvert.DeserializeObject<CachedProductList>(cachedListInfo);
            var missingIds = new List<Guid>();

            if (listInfo is not null)
            {
                var products = new List<ProductResponse>();

                await _resiliencePolicy.ExecuteAsync(async () =>
                {
                    var db = _connectionMultiplexer.GetDatabase();
                    // Tạo mảng RedisKey cho tất cả các ProductId trong danh sách
                    var redisKeys = listInfo.ProductIds
                        .Select(id => (RedisKey)$"ProductResponse:{id}")
                        .ToArray();

                    // Lấy giá trị cache cho tất cả các sản phẩm trong danh sách
                    var cachedValues = await db.StringGetAsync(redisKeys);
                    for (int i = 0; i < listInfo.ProductIds.Count; i++)
                    {
                        var id = listInfo.ProductIds[i];
                        var cachedValue = cachedValues[i];

                        if (cachedValue.HasValue && !cachedValue.IsNullOrEmpty)
                        {
                            var product = JsonConvert.DeserializeObject<ProductResponse>(cachedValue.ToString());
                            if (product != null)
                            {
                                products.Add(product);
                            }
                        }
                        else
                        {
                            missingIds.Add(id);
                        }

                        // Xử lý cache miss
                        if (missingIds.Any())
                        {
                            var missingProducts = await _decorated.GetProductsByIds(
                                missingIds.Select(id => ProductId.FromGuid(id)),
                                CancellationToken.None);

                            var batch = db.CreateBatch();
                            var batchTasks = new List<Task>();

                            foreach (var product in missingProducts)
                            {
                                string productCacheKey = $"ProductResponse:{product.ProductId}";

                                batchTasks.Add(batch.StringSetAsync(
                                    productCacheKey,
                                    JsonConvert.SerializeObject(product),
                                    _entityCacheTtl));

                                products.Add(product);
                            }

                            batch.Execute();
                            await Task.WhenAll(batchTasks);
                        }
                    }

                    return null;
                });

                return new PagedList<ProductResponse>(
                    products,
                    listInfo.Page,
                    listInfo.PageSize,
                    listInfo.TotalCount);
            }
        }

        var result = await _decorated.GetAllProductWithVariantList(
            filterProductStatus,
            filterCategory,
            searchTerm,
            sortColumn,
            sortOrder,
            page,
            pageSize);

        if (result.Items.Any())
        {
            await _resiliencePolicy.ExecuteAsync(async () =>
            {
                var db = _connectionMultiplexer.GetDatabase();
                var batch = db.CreateBatch();
                var batchTasks = new List<Task>();

                foreach (var product in result.Items)
                {
                    string productCacheKey = $"ProductResponse:{product.ProductId}";

                    batchTasks.Add(batch.StringSetAsync(
                        productCacheKey,
                        JsonConvert.SerializeObject(product),
                        _entityCacheTtl));
                }

                batch.Execute();
                await Task.WhenAll(batchTasks);

                var listInfoToCache = new CachedProductList(
                    result.Items.Select(p => p.ProductId).ToList(),
                    result.Page,
                    result.PageSize,
                    result.TotalCount);

                // Ghi danh sách ProductId cùng với thông tin phân trang vào cache để lần sau có thể lấy nhanh
                await _distributedCache.SetStringAsync(
                    cacheKey,
                    JsonConvert.SerializeObject(listInfoToCache));

                return null;
            });
        }

        return result;
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

    public async Task<List<ProductResponse>> GetProductsByIds(IEnumerable<ProductId> productIds, CancellationToken cancellationToken = default)
    {
        return await _decorated.GetProductsByIds(productIds, cancellationToken);
    }
}
