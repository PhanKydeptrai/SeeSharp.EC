using Application.DTOs.Category;
using Application.Features.Pages;
using Application.IServices;
using Domain.Entities.Categories;
using Infrastructure.Helper;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Polly;
using Polly.Registry;
using SharedKernel.Constants;
using StackExchange.Redis;

namespace Infrastructure.Services.CategoryServices;

internal sealed class CategoryQueryServicesDecorated : ICategoryQueryServices
{
    private static readonly DistributedCacheEntryOptions CategoryCacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
    };

    private static readonly DistributedCacheEntryOptions CategoryInfoCacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
    };

    private readonly TimeSpan _entityCacheTtl = TimeSpan.FromMinutes(30);

    private readonly ICategoryQueryServices _decorated;
    private readonly IDistributedCache _cache;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly ICacheKeyGenerator _cacheKeyGenerator;
    private readonly IAsyncPolicy<string?> _resiliencePolicy;

    public CategoryQueryServicesDecorated(
        ICategoryQueryServices decorated,
        IDistributedCache cache,
        IConnectionMultiplexer connectionMultiplexer,
        ICacheKeyGenerator cacheKeyGenerator,
        IReadOnlyPolicyRegistry<string> policyRegistry)
    {
        _decorated = decorated;
        _cache = cache;
        _connectionMultiplexer = connectionMultiplexer;
        _cacheKeyGenerator = cacheKeyGenerator;
        _resiliencePolicy = policyRegistry.Get<IAsyncPolicy<string?>>(Strategy.RedisStrategy);
    }

    public async Task<CategoryResponse?> GetById(
        CategoryId categoryId,
        CancellationToken cancellationToken = default)
    {
        string cacheKey = $"CategoryResponse:{categoryId.Value}";

        string? cachedData = await _resiliencePolicy.ExecuteAsync(
            async () =>
            {
                string? cachedCategory = await _cache.GetStringAsync(cacheKey, cancellationToken);
                return cachedCategory;
            }
        );

        if (!string.IsNullOrEmpty(cachedData))
        {
            return JsonConvert.DeserializeObject<CategoryResponse>(cachedData);
        }

        var category = await _decorated.GetById(categoryId, cancellationToken);

        if (category is not null)
        {
            await _resiliencePolicy.ExecuteAsync(async () =>
            {
                await _cache.SetStringAsync(
                    cacheKey,
                    JsonConvert.SerializeObject(category),
                    CategoryCacheOptions,
                    cancellationToken);
                return null;
            });
        }

        return category;
    }

    public async Task<CategoryResponse?> GetCategoryDetail(
        CategoryId categoryId,
        CancellationToken cancellationToken = default)
    {
        string cacheKey = $"CategoryDetailResponse:{categoryId.Value}";

        string? cachedCategoryDetail = await _resiliencePolicy.ExecuteAsync(async () =>
        {
            return await _cache.GetStringAsync(cacheKey, cancellationToken);
        });

        if (!string.IsNullOrEmpty(cachedCategoryDetail))
        {
            return JsonConvert.DeserializeObject<CategoryResponse>(cachedCategoryDetail);
        }

        var categoryDetail = await _decorated.GetCategoryDetail(categoryId, cancellationToken);

        if (categoryDetail is not null)
        {
            await _resiliencePolicy.ExecuteAsync(async () =>
            {
                await _cache.SetStringAsync(
                    cacheKey,
                    JsonConvert.SerializeObject(categoryDetail),
                    CategoryCacheOptions,
                    cancellationToken);
                return null;
            });
        }

        return categoryDetail;
    }

    public async Task<List<CategoryInfo>> GetCategoryInfo()
    {
        string cacheKey = "CategoryInfo:All";

        string? cachedCategoryInfo = await _resiliencePolicy.ExecuteAsync(async () =>
        {
            return await _cache.GetStringAsync(cacheKey);
        });

        if (!string.IsNullOrEmpty(cachedCategoryInfo))
        {
            return JsonConvert.DeserializeObject<List<CategoryInfo>>(cachedCategoryInfo) ?? [];
        }

        var categoryInfo = await _decorated.GetCategoryInfo();

        await _resiliencePolicy.ExecuteAsync(async () =>
        {
            await _cache.SetStringAsync(
                cacheKey,
                JsonConvert.SerializeObject(categoryInfo),
                CategoryInfoCacheOptions);
            return null;
        });

        return categoryInfo;
    }

    public async Task<bool> IsCategoryNameExist(
        CategoryId? categoryId,
        CategoryName categoryName,
        CancellationToken cancellationToken = default)
    {
        return await _decorated.IsCategoryNameExist(categoryId ?? null, categoryName, cancellationToken);
    }

    public async Task<bool> IsCategoryStatusNotDeleted(
        CategoryId categoryId,
        CancellationToken cancellationToken = default)
    {
        return await _decorated.IsCategoryStatusNotDeleted(categoryId, cancellationToken);
    }

    public async Task<List<CategoryResponse>> GetCategoriesByIds(
        IEnumerable<CategoryId> categoryIds,
        CancellationToken cancellationToken = default)
    {
        return await _decorated.GetCategoriesByIds(categoryIds, cancellationToken);
    }

    private record CachedCategoryList(List<Guid> CategoryIds, int Page, int PageSize, int TotalCount);

    public async Task<PagedList<CategoryResponse>> PagedList(
        string? filter,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize)
    {
        string cacheKey = _cacheKeyGenerator.CreateCacheKey("CategoryList", filter, null, sortColumn, sortOrder, page, pageSize, searchTerm);

        string? cachedListInfo = await _resiliencePolicy.ExecuteAsync(
            async () =>
            {
                // Lấy dữ liệu danh sách từ cache
                string? cachedListInfo = await _cache.GetStringAsync(cacheKey);
                return cachedListInfo;
            }
        );

        if (!string.IsNullOrEmpty(cachedListInfo)) // Nếu có cache, tiếp tục lấy chi tiết từng category
        {
            var listInfo = JsonConvert.DeserializeObject<CachedCategoryList>(cachedListInfo);
            var missingIds = new List<Guid>();

            if (listInfo is not null)
            {
                var categories = new List<CategoryResponse>();

                await _resiliencePolicy.ExecuteAsync(async () =>
                {
                    var db = _connectionMultiplexer.GetDatabase();
                    // Tạo mảng RedisKey cho tất cả các CategoryId trong danh sách
                    var redisKeys = listInfo.CategoryIds
                        .Select(id => (RedisKey)$"CategoryResponse:{id}")
                        .ToArray();

                    // Lấy giá trị cache cho tất cả các categories trong danh sách
                    var cachedValues = await db.StringGetAsync(redisKeys);
                    for (int i = 0; i < listInfo.CategoryIds.Count; i++)
                    {
                        var id = listInfo.CategoryIds[i];
                        var cachedValue = cachedValues[i];

                        if (cachedValue.HasValue && !cachedValue.IsNullOrEmpty)
                        {
                            var category = JsonConvert.DeserializeObject<CategoryResponse>(cachedValue.ToString());
                            if (category != null)
                            {
                                categories.Add(category);
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
                        var missingCategories = await _decorated.GetCategoriesByIds(
                            missingIds.Select(id => CategoryId.FromGuid(id)),
                            CancellationToken.None);

                        var batch = db.CreateBatch();
                        var batchTasks = new List<Task>();

                        foreach (var category in missingCategories)
                        {
                            string categoryCacheKey = $"CategoryResponse:{category.categoryId}";

                            batchTasks.Add(batch.StringSetAsync(
                                categoryCacheKey,
                                JsonConvert.SerializeObject(category),
                                _entityCacheTtl));

                            categories.Add(category);
                        }

                        batch.Execute();
                        await Task.WhenAll(batchTasks);
                    }

                    return null;
                });

                return new PagedList<CategoryResponse>(
                    categories,
                    listInfo.Page,
                    listInfo.PageSize,
                    listInfo.TotalCount);
            }
        }

        var result = await _decorated.PagedList(filter, searchTerm, sortColumn, sortOrder, page, pageSize);

        if (result.Items.Any())
        {
            await _resiliencePolicy.ExecuteAsync(async () =>
            {
                var db = _connectionMultiplexer.GetDatabase();
                var batch = db.CreateBatch();
                var batchTasks = new List<Task>();

                foreach (var category in result.Items)
                {
                    string categoryCacheKey = $"CategoryResponse:{category.categoryId}";

                    batchTasks.Add(batch.StringSetAsync(
                        categoryCacheKey,
                        JsonConvert.SerializeObject(category),
                        _entityCacheTtl));
                }

                batch.Execute();
                await Task.WhenAll(batchTasks);

                var listInfoToCache = new CachedCategoryList(
                    result.Items.Select(c => c.categoryId).ToList(),
                    result.Page,
                    result.PageSize,
                    result.TotalCount);

                // Ghi danh sách CategoryId cùng với thông tin phân trang vào cache để lần sau có thể lấy nhanh
                await _cache.SetStringAsync(
                    cacheKey,
                    JsonConvert.SerializeObject(listInfoToCache),
                    CategoryCacheOptions);

                return null;
            });
        }

        return result;
    }
}

