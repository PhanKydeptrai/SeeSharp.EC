using Application.DTOs.Category;
using Application.Features.Pages;
using Application.Helper;
using Application.IServices;
using Domain.Entities.Categories;
using SharedKernel.Constants;

namespace Infrastructure.Services.CategoryServices;

internal sealed class CategoryQueryServicesDecorated : ICategoryQueryServices
{
    private readonly ICategoryQueryServices _decorated;
    private readonly IRedisCacheService _cacheService;
    private readonly ICacheKeyGenerator _cacheKeyGenerator;

    private readonly TimeSpan _entityCacheTtl = TimeSpan.FromMinutes(30);
    private readonly TimeSpan _listCacheTtl = TimeSpan.FromMinutes(10);
    private readonly TimeSpan _categoryInfoCacheTtl = TimeSpan.FromMinutes(30);

    public CategoryQueryServicesDecorated(
        ICategoryQueryServices decorated,
        IRedisCacheService cacheService,
        ICacheKeyGenerator cacheKeyGenerator)
    {
        _decorated = decorated;
        _cacheService = cacheService;
        _cacheKeyGenerator = cacheKeyGenerator;
    }

    public async Task<CategoryResponse?> GetById(
        CategoryId categoryId,
        CancellationToken cancellationToken = default)
    {
        string cacheKey = $"CategoryResponse:{categoryId.Value}";

        CategoryResponse? cachedData = await _cacheService.GetAsync<CategoryResponse>(cacheKey, cancellationToken);

        if (cachedData is not null)
        {
            return cachedData;
        }

        var category = await _decorated.GetById(categoryId, cancellationToken);

        if (category is not null)
        {
            await _cacheService.SetAsync(cacheKey, category, _entityCacheTtl, cancellationToken);
        }

        return category;
    }

    public async Task<CategoryResponse?> GetCategoryDetail(
        CategoryId categoryId,
        CancellationToken cancellationToken = default)
    {
        string cacheKey = $"CategoryDetailResponse:{categoryId.Value}";

        CategoryResponse? cachedCategoryDetail = await _cacheService.GetAsync<CategoryResponse>(cacheKey, cancellationToken);

        if (cachedCategoryDetail is not null)
        {
            return cachedCategoryDetail;
        }

        var categoryDetail = await _decorated.GetCategoryDetail(categoryId, cancellationToken);

        if (categoryDetail is not null)
        {
            await _cacheService.SetAsync(cacheKey, categoryDetail, _entityCacheTtl, cancellationToken);
        }

        return categoryDetail;
    }

    public async Task<List<CategoryInfo>> GetCategoryInfo()
    {
        string cacheKey = "CategoryInfo:All";

        var cachedCategoryInfo = await _cacheService.GetAsync<List<CategoryInfo>>(cacheKey);

        if (cachedCategoryInfo is not null)
        {
            return cachedCategoryInfo;
        }

        var categoryInfo = await _decorated.GetCategoryInfo();

        await _cacheService.SetAsync(cacheKey, categoryInfo, _categoryInfoCacheTtl);

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
        string cacheKey = await _cacheKeyGenerator.CreateCacheKeyAsync("CategoryList", filter, null, sortColumn, sortOrder, page, pageSize, searchTerm);

        CachedCategoryList? listInfo = await _cacheService.GetAsync<CachedCategoryList>(cacheKey);

        if (listInfo is not null)
        {
            var redisKeys = listInfo.CategoryIds.Select(id => $"CategoryResponse:{id}").ToList();
            var cachedCategories = await _cacheService.GetManyAsync<CategoryResponse>(redisKeys);

            var categories = new List<CategoryResponse>();
            var missingIds = new List<Guid>();

            for (int i = 0; i < listInfo.CategoryIds.Count; i++)
            {
                if (cachedCategories[i] is not null)
                {
                    categories.Add(cachedCategories[i]!);
                }
                else
                {
                    missingIds.Add(listInfo.CategoryIds[i]);
                }
            }

            if (missingIds.Any())
            {
                var missingCategories = await _decorated.GetCategoriesByIds(
                    missingIds.Select(id => CategoryId.FromGuid(id)),
                    CancellationToken.None);

                if (missingCategories.Any())
                {
                    var categoriesToCache = missingCategories.ToDictionary(
                        c => $"CategoryResponse:{c.categoryId}",
                        c => c);

                    await _cacheService.SetManyAsync(categoriesToCache, _entityCacheTtl);
                    categories.AddRange(missingCategories);
                }
            }

            return new PagedList<CategoryResponse>(
                categories,
                listInfo.Page,
                listInfo.PageSize,
                listInfo.TotalCount);
        }

        var result = await _decorated.PagedList(filter, searchTerm, sortColumn, sortOrder, page, pageSize);

        if (result.Items.Any())
        {
            var listToCache = new CachedCategoryList(
                result.Items.Select(c => c.categoryId).ToList(),
                result.Page,
                result.PageSize,
                result.TotalCount);

            await _cacheService.SetAsync(cacheKey, listToCache, _listCacheTtl);

            var categoriesToCache = result.Items.ToDictionary(
                c => $"CategoryResponse:{c.categoryId}",
                c => c);

            await _cacheService.SetManyAsync(categoriesToCache, _entityCacheTtl);
        }

        return result;
    }

    public async Task<GetCategoryIdListResponse> GetCategoryIdList(string? filter, string? searchTerm, string? sortColumn, string? sortOrder, int? page, int? pageSize)
    {
        return await _decorated.GetCategoryIdList(filter, searchTerm, sortColumn, sortOrder, page, pageSize);
    }
}

