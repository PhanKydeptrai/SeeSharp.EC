using Application.DTOs.Category;
using Application.Features.Pages;
using Application.IServices;
using Domain.Entities.Categories;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

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

    public CategoryQueryServicesDecorated(
        ICategoryQueryServices decorated,
        IDistributedCache cache)
    {
        _decorated = decorated;
        _cache = cache;
    }

    public async Task<CategoryResponse?> GetById(
        CategoryId categoryId, 
        CancellationToken cancellationToken = default)
    {
        string cacheKey = BuildCategoryByIdCacheKey(await GetCacheVersionAsync(cancellationToken), categoryId);
        string? cachedCategory = await _cache.GetStringAsync(cacheKey, cancellationToken);

        if (!string.IsNullOrEmpty(cachedCategory))
        {
            return JsonConvert.DeserializeObject<CategoryResponse>(cachedCategory);
        }

        var category = await _decorated.GetById(categoryId, cancellationToken);

        if (category is null)
        {
            return null;
        }

        await _cache.SetStringAsync(
            cacheKey,
            JsonConvert.SerializeObject(category),
            CategoryCacheOptions,
            cancellationToken);

        return category;
    }

    public async Task<CategoryResponse?> GetCategoryDetail(
        CategoryId categoryId, 
        CancellationToken cancellationToken = default)
    {
        string cacheKey = BuildCategoryDetailCacheKey(await GetCacheVersionAsync(cancellationToken), categoryId);
        string? cachedCategoryDetail = await _cache.GetStringAsync(cacheKey, cancellationToken);

        if (!string.IsNullOrEmpty(cachedCategoryDetail))
        {
            return JsonConvert.DeserializeObject<CategoryResponse>(cachedCategoryDetail);
        }

        var categoryDetail = await _decorated.GetCategoryDetail(categoryId, cancellationToken);

        if (categoryDetail is null)
        {
            return null;
        }

        await _cache.SetStringAsync(
            cacheKey,
            JsonConvert.SerializeObject(categoryDetail),
            CategoryCacheOptions,
            cancellationToken);

        return categoryDetail;
    }

    public async Task<List<CategoryInfo>> GetCategoryInfo()
    {
        string cacheKey = BuildCategoryInfoCacheKey(await GetCacheVersionAsync());
        string? cachedCategoryInfo = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedCategoryInfo))
        {
            return JsonConvert.DeserializeObject<List<CategoryInfo>>(cachedCategoryInfo) ?? [];
        }

        var categoryInfo = await _decorated.GetCategoryInfo();

        await _cache.SetStringAsync(
            cacheKey,
            JsonConvert.SerializeObject(categoryInfo),
            CategoryInfoCacheOptions);

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
        string cacheKey = BuildPagedListCacheKey(
            await GetCacheVersionAsync(),
            filter,
            searchTerm,
            sortColumn,
            sortOrder,
            page,
            pageSize);
        string? cachedPagedList = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedPagedList))
        {
            return JsonConvert.DeserializeObject<PagedList<CategoryResponse>>(cachedPagedList)
                ?? new PagedList<CategoryResponse>([], page ?? 1, pageSize ?? 10, 0);
        }

        var pagedCategories = await _decorated.PagedList(filter, searchTerm, sortColumn, sortOrder, page, pageSize);

        await _cache.SetStringAsync(
            cacheKey,
            JsonConvert.SerializeObject(pagedCategories),
            CategoryCacheOptions);

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
