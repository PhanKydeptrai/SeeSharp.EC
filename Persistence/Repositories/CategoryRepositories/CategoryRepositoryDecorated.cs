using Domain.Entities.Categories;
using Domain.IRepositories.CategoryRepositories;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Persistence.Repositories.CategoryRepositories;

internal sealed class CategoryRepositoryDecorated : ICategoryRepository
{
    private static readonly DistributedCacheEntryOptions RepositoryCacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
    };

    private readonly ICategoryRepository _decorated;
    private readonly IDistributedCache _cache;

    public CategoryRepositoryDecorated(
        ICategoryRepository decorated, 
        IDistributedCache cache)
    {
        _decorated = decorated;
        _cache = cache;
    }

    public async Task AddCategoryToPosgreSQL(Category category)
    {
        await _decorated.AddCategoryToPosgreSQL(category);
        
        string cacheKeyIsExist = $"CategoryRepository:IsExist:{category.CategoryId.Value}";
        string cacheKeyCategory = $"CategoryRepository:Category:{category.CategoryId.Value}";
        
        await _cache.RemoveAsync(cacheKeyIsExist);
        await _cache.RemoveAsync(cacheKeyCategory);
    }

    public async Task<Category?> GetCategoryByIdFromPostgreSQL(CategoryId categoryId, CancellationToken cancellationToken = default)
    {
        string cacheKey = $"CategoryRepository:Category:{categoryId.Value}";
        string? cachedCategory = await _cache.GetStringAsync(cacheKey, cancellationToken);

        if (!string.IsNullOrEmpty(cachedCategory))
        {
            return JsonConvert.DeserializeObject<Category>(cachedCategory);
        }

        var category = await _decorated.GetCategoryByIdFromPostgreSQL(categoryId, cancellationToken);
        
        if (category is not null)
        {
            await _cache.SetStringAsync(
                cacheKey,
                JsonConvert.SerializeObject(category),
                RepositoryCacheOptions,
                cancellationToken);
        }

        return category; 
    }

    public async Task<bool> IsCategoryIdExist(CategoryId categoryId, CancellationToken cancellationToken = default)
    {
        string cacheKey = $"CategoryRepository:IsExist:{categoryId.Value}";
        string? cachedResult = await _cache.GetStringAsync(cacheKey, cancellationToken);

        if (!string.IsNullOrEmpty(cachedResult))
        {
            return bool.Parse(cachedResult);
        }

        bool isExist = await _decorated.IsCategoryIdExist(categoryId, cancellationToken);

        await _cache.SetStringAsync(
            cacheKey,
            isExist.ToString(),
            RepositoryCacheOptions,
            cancellationToken);

        return isExist;
    }
}
