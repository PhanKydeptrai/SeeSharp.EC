using Application.DTOs.Category;
using Application.Features.Pages;
using Application.IServices;
using Domain.Entities.Categories;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Polly;
using Polly.Registry;
using SharedKernel.Constants;

namespace Infrastructure.Services.CategoryServices;

internal class CategoryQueryServicesDecorated : ICategoryQueryServices
{
    private const string CategoryCacheVersionKey = "Category:CacheVersion";

    private static readonly DistributedCacheEntryOptions CategoryCacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
    };

    private static readonly DistributedCacheEntryOptions CategoryInfoCacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
    };

    private readonly ICategoryQueryServices _decorated;
    private readonly IDistributedCache _cache;
    private readonly IAsyncPolicy<string?> _resiliencePolicy;

    public CategoryQueryServicesDecorated(
        ICategoryQueryServices decorated,
        IDistributedCache cache,
        IReadOnlyPolicyRegistry<string> policyRegistry)
    {
        _decorated = decorated;
        _cache = cache;
        _resiliencePolicy = policyRegistry.Get<IAsyncPolicy<string?>>(Strategy.RedisStrategy);
    }

    public async Task<CategoryResponse?> GetById(
        CategoryId categoryId,
        CancellationToken cancellationToken = default)
    {

        string? cachedData = await _resiliencePolicy.ExecuteAsync(
            async () =>
            {
                string cacheKey = BuildCategoryByIdCacheKey(await GetCacheVersionAsync(cancellationToken), categoryId);
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
                string cacheKey = BuildCategoryByIdCacheKey(await GetCacheVersionAsync(cancellationToken), categoryId);
                var serializedData = JsonConvert.SerializeObject(category);
                
                await _cache.SetStringAsync(cacheKey, serializedData, CategoryCacheOptions, cancellationToken);
                return null;
            });
        }

        return category;
    }

    public async Task<CategoryResponse?> GetCategoryDetail(
        CategoryId categoryId,
        CancellationToken cancellationToken = default)
    {
        string? cachedCategoryDetail = await _resiliencePolicy.ExecuteAsync(async () =>
        {
            string cacheKey = BuildCategoryDetailCacheKey(await GetCacheVersionAsync(cancellationToken), categoryId);
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
                string cacheKey = BuildCategoryDetailCacheKey(await GetCacheVersionAsync(cancellationToken), categoryId);
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
        string? cachedCategoryInfo = await _resiliencePolicy.ExecuteAsync(async () =>
        {
            string cacheKey = BuildCategoryInfoCacheKey(await GetCacheVersionAsync());
            return await _cache.GetStringAsync(cacheKey);
        });

        if (!string.IsNullOrEmpty(cachedCategoryInfo))
        {
            return JsonConvert.DeserializeObject<List<CategoryInfo>>(cachedCategoryInfo) ?? [];
        }

        var categoryInfo = await _decorated.GetCategoryInfo();

        await _resiliencePolicy.ExecuteAsync(async () =>
        {
            string cacheKey = BuildCategoryInfoCacheKey(await GetCacheVersionAsync());
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

    public async Task<PagedList<CategoryResponse>> PagedList(
        string? filter,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize)
    {
        string? cachedPagedList = await _resiliencePolicy.ExecuteAsync(async () =>
        {
            string cacheKey = BuildPagedListCacheKey(
                await GetCacheVersionAsync(),
                filter,
                searchTerm,
                sortColumn,
                sortOrder,
                page,
                pageSize);
            return await _cache.GetStringAsync(cacheKey);
        });

        if (!string.IsNullOrEmpty(cachedPagedList))
        {
            return JsonConvert.DeserializeObject<PagedList<CategoryResponse>>(cachedPagedList)
                ?? new PagedList<CategoryResponse>([], page ?? 1, pageSize ?? 10, 0);
        }

        var pagedCategories = await _decorated.PagedList(filter, searchTerm, sortColumn, sortOrder, page, pageSize);

        await _resiliencePolicy.ExecuteAsync(async () =>
        {
            string cacheKey = BuildPagedListCacheKey(
                await GetCacheVersionAsync(),
                filter,
                searchTerm,
                sortColumn,
                sortOrder,
                page,
                pageSize);
            await _cache.SetStringAsync(
                cacheKey,
                JsonConvert.SerializeObject(pagedCategories),
                CategoryCacheOptions);
            return null;
        });

        return pagedCategories;
    }

    private async Task<string> GetCacheVersionAsync(CancellationToken cancellationToken = default)
    {
        return await _cache.GetStringAsync(CategoryCacheVersionKey, cancellationToken) ?? "v1";
    }

    private static string BuildCategoryByIdCacheKey(string version, CategoryId categoryId)
    {
        return $"Category:{version}:Response:{categoryId.Value}";
    }

    private static string BuildCategoryDetailCacheKey(string version, CategoryId categoryId)
    {
        return $"Category:{version}:DetailResponse:{categoryId.Value}";
    }

    private static string BuildCategoryInfoCacheKey(string version)
    {
        return $"Category:{version}:Info:All";
    }

    private static string BuildPagedListCacheKey(
        string version,
        string? filter,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize)
    {
        return $"Category:{version}:PagedList:{filter ?? "all"}:{searchTerm ?? "none"}:{sortColumn ?? "categoryid"}:{sortOrder ?? "asc"}:{page ?? 1}:{pageSize ?? 10}";
    }
}
