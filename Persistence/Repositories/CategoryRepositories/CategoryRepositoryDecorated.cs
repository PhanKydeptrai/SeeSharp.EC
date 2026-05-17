using Application.IServices;
using Domain.Entities.Categories;
using Domain.IRepositories.CategoryRepositories;

namespace Persistence.Repositories.CategoryRepositories;

internal sealed class CategoryRepositoryDecorated : ICategoryRepository
{
    private readonly TimeSpan _entityCacheTtl = TimeSpan.FromMinutes(10);

    private readonly ICategoryRepository _decorated;
    private readonly IRedisCacheService _cacheService;

    public CategoryRepositoryDecorated(
        ICategoryRepository decorated, 
        IRedisCacheService cacheService)
    {
        _decorated = decorated;
        _cacheService = cacheService;
    }

    public async Task AddCategoryToPosgreSQL(Category category)
    {
        await _decorated.AddCategoryToPosgreSQL(category);
        
        string cacheKeyIsExist = $"CategoryRepository:IsExist:{category.CategoryId.Value}";
        string cacheKeyCategory = $"CategoryRepository:Category:{category.CategoryId.Value}";
        
        await _cacheService.RemoveAsync(cacheKeyIsExist);
        await _cacheService.RemoveAsync(cacheKeyCategory);
    }

    public async Task<Category?> GetCategoryByIdFromPostgreSQL(CategoryId categoryId, CancellationToken cancellationToken = default)
    {
        string cacheKey = $"CategoryRepository:Category:{categoryId.Value}";
        Category? cachedCategory = await _cacheService.GetAsync<Category>(cacheKey, cancellationToken);

        if (cachedCategory is not null)
        {
            return cachedCategory;
        }

        var category = await _decorated.GetCategoryByIdFromPostgreSQL(categoryId, cancellationToken);
        
        if (category is not null)
        {
            await _cacheService.SetAsync(
                cacheKey,
                category,
                _entityCacheTtl,
                cancellationToken);
        }

        return category; 
    }

    public async Task<bool> IsCategoryIdExist(CategoryId categoryId, CancellationToken cancellationToken = default)
    {
        string cacheKey = $"CategoryRepository:IsExist:{categoryId.Value}";
        bool? cachedResult = await _cacheService.GetAsync<bool?>(cacheKey, cancellationToken);

        if (cachedResult is not null)
        {
            return cachedResult.Value;
        }

        bool isExist = await _decorated.IsCategoryIdExist(categoryId, cancellationToken);

        await _cacheService.SetAsync(
            cacheKey,
            isExist,
            _entityCacheTtl,
            cancellationToken);

        return isExist;
    }
}
