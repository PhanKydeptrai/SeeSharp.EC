using Application.DTOs.Product;
using Application.Features.Pages;
using Application.IServices;
using Domain.Entities.Products;
using Domain.Entities.ProductVariants;
using Infrastructure.Helper;
using Newtonsoft.Json;
using Polly;
using Polly.Registry;
using SharedKernel.Constants;
using StackExchange.Redis;

namespace Infrastructure.Services.ProductServices;

internal sealed class ProductQueryServicesDecorated : IProductQueryServices
{
    private readonly IProductQueryServices _decorated;
    private readonly IDatabase _redisDb;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly ICacheKeyGenerator _cacheKeyGenerator;
    private readonly IAsyncPolicy<string?> _resiliencePolicy;

    private readonly TimeSpan _entityCacheTtl = TimeSpan.FromMinutes(30);

    public ProductQueryServicesDecorated(
        IProductQueryServices decorated,
        IConnectionMultiplexer connectionMultiplexer,
        ICacheKeyGenerator cacheKeyGenerator,
        IReadOnlyPolicyRegistry<string> policyRegistry)
    {
        _decorated = decorated;
        _connectionMultiplexer = connectionMultiplexer;
        _cacheKeyGenerator = cacheKeyGenerator;
        _resiliencePolicy = policyRegistry.Get<IAsyncPolicy<string?>>(Strategy.RedisStrategy);
        _redisDb = connectionMultiplexer.GetDatabase();
    }

    public async Task<ProductResponse?> GetProductWithVariantListById(
        ProductId productId,
        CancellationToken cancellationToken = default)
    {
        string cacheKey = $"ProductResponse:{productId.Value}";

        string? cachedProduct = await _resiliencePolicy.ExecuteAsync(
            async () =>
            {
                // Lấy dữ liệu danh sách từ cache
                var cachedProduct = await _redisDb.StringGetAsync(cacheKey);
                return cachedProduct;
            }
        );

        if (string.IsNullOrEmpty(cachedProduct))
        {
            ProductResponse? product = await _decorated.GetProductWithVariantListById(productId, cancellationToken);

            if (product is null) return product;

            await _resiliencePolicy.ExecuteAsync(async () =>
            {
                await _redisDb.StringSetAsync(
                    cacheKey,
                    JsonConvert.SerializeObject(product));

                return null;
            });

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
        string? cachedListInfo = await _resiliencePolicy.ExecuteAsync(
            async () =>
            {
                
                string? cachedListInfo = await _redisDb.StringGetAsync(cacheKey);
                return cachedListInfo;
            }
        );


        if (!string.IsNullOrEmpty(cachedListInfo)) //Nếu tìm thấy list id trong cache, tiếp tục lấy chi tiết từng sản phẩm
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
                            if (product is not null)
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

                var results= new PagedList<ProductResponse>(
                    products,
                    listInfo.Page,
                    listInfo.PageSize,
                    listInfo.TotalCount);

                return results;
            }
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
            await _resiliencePolicy.ExecuteAsync(async () =>
            {
                await _redisDb.StringSetAsync(
                    cacheKey,
                    JsonConvert.SerializeObject(
                        new CachedProductList(
                            result.ProductIds, 
                            result.Page, 
                            result.PageSize, 
                            result.TotalCount)
                ));

                return null;
            });
        }

        // Query DB lấy dữ liệu chi tiết
        var productList = await _decorated.GetProductsByIds(
            result.ProductIds.Select(id => ProductId.FromGuid(id)),
            CancellationToken.None);

        // Ráp dữ liệu chi tiết vào kết quả trả về
        var response = new PagedList<ProductResponse>(
            productList,
            result.Page,
            result.PageSize,
            result.TotalCount);

        if (productList.Any()) // Nếu có kết quả, cache thông tin list id sản phẩm cùng với phân trang để lần sau truy vấn nhanh hơn
        {
            await _resiliencePolicy.ExecuteAsync(async () =>
            {
                var db = _connectionMultiplexer.GetDatabase();
                var batch = db.CreateBatch();
                var batchTasks = new List<Task>();

                foreach (var product in productList)
                {
                    string productCacheKey = $"ProductResponse:{product.ProductId}";

                    batchTasks.Add(batch.StringSetAsync(
                        productCacheKey,
                        JsonConvert.SerializeObject(product),
                        _entityCacheTtl));
                }

                batch.Execute();
                await Task.WhenAll(batchTasks); 

                return null;
            });
        }

        return response;
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

        string? cachedListInfo = await _resiliencePolicy.ExecuteAsync(
            async () =>
            {
                // Lấy dữ liệu danh sách từ cache
                string? cachedListInfo = await _redisDb.StringGetAsync(cacheKey);
                return cachedListInfo;
            }
        );

        if (!string.IsNullOrEmpty(cachedListInfo)) // Nếu có cache, tiếp tục lấy chi tiết từng variant
        {
            var listInfo = JsonConvert.DeserializeObject<CachedVariantList>(cachedListInfo);
            var missingIds = new List<Guid>();

            if (listInfo is not null)
            {
                var variants = new List<ProductVariantResponse>();

                await _resiliencePolicy.ExecuteAsync(async () =>
                {
                    var db = _connectionMultiplexer.GetDatabase();
                    // Tạo mảng RedisKey cho tất cả các VariantId trong danh sách
                    var redisKeys = listInfo.VariantIds
                        .Select(id => (RedisKey)$"ProductVariantResponse:{id}")
                        .ToArray();

                    // Lấy giá trị cache cho tất cả các variants trong danh sách
                    var cachedValues = await db.StringGetAsync(redisKeys);
                    for (int i = 0; i < listInfo.VariantIds.Count; i++)
                    {
                        var id = listInfo.VariantIds[i];
                        var cachedValue = cachedValues[i];

                        if (cachedValue.HasValue && !cachedValue.IsNullOrEmpty)
                        {
                            var variant = JsonConvert.DeserializeObject<ProductVariantResponse>(cachedValue.ToString());
                            if (variant != null)
                            {
                                variants.Add(variant);
                            }
                        }
                        else
                        {
                            missingIds.Add(id);
                        }
                    }

                    // Xử lý cache miss
                    if (missingIds.Any())
                    {
                        var missingVariants = await _decorated.GetVariantsByIds(
                            missingIds.Select(id => ProductVariantId.FromGuid(id)),
                            CancellationToken.None);

                        var batch = db.CreateBatch();
                        var batchTasks = new List<Task>();

                        foreach (var variant in missingVariants)
                        {
                            string variantCacheKey = $"ProductVariantResponse:{variant.ProductVariantId}";

                            batchTasks.Add(batch.StringSetAsync(
                                variantCacheKey,
                                JsonConvert.SerializeObject(variant),
                                _entityCacheTtl));

                            variants.Add(variant);
                        }

                        batch.Execute();
                        await Task.WhenAll(batchTasks);
                    }

                    return null;
                });

                return new PagedList<ProductVariantResponse>(
                    variants,
                    listInfo.Page,
                    listInfo.PageSize,
                    listInfo.TotalCount);
            }
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
            await _resiliencePolicy.ExecuteAsync(async () =>
            {
                await _redisDb.StringSetAsync(
                    cacheKey,
                    JsonConvert.SerializeObject(
                        new CachedVariantList(
                            result.VariantIds, 
                            result.Page, 
                            result.PageSize, 
                            result.TotalCount)
                ));

                return null;
            });
        }

        // Query DB lấy dữ liệu chi tiết
        var variantList = await _decorated.GetVariantsByIds(
            result.VariantIds.Select(id => ProductVariantId.FromGuid(id)),
            CancellationToken.None);

        // Ráp dữ liệu chi tiết vào kết quả trả về
        var response = new PagedList<ProductVariantResponse>(
            variantList,
            result.Page,
            result.PageSize,
            result.TotalCount);

        if (variantList.Any()) // Nếu có kết quả, cache thông tin chi tiết từng variant để lần sau truy vấn nhanh hơn
        {
            await _resiliencePolicy.ExecuteAsync(async () =>
            {
                var db = _connectionMultiplexer.GetDatabase();
                var batch = db.CreateBatch();
                var batchTasks = new List<Task>();

                foreach (var variant in variantList)
                {
                    string variantCacheKey = $"ProductVariantResponse:{variant.ProductVariantId}";

                    batchTasks.Add(batch.StringSetAsync(
                        variantCacheKey,
                        JsonConvert.SerializeObject(variant),
                        _entityCacheTtl));
                }

                batch.Execute();
                await Task.WhenAll(batchTasks); 

                return null;
            });
        }

        return response;
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
