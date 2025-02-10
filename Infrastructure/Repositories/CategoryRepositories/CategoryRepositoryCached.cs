using Domain.Database.PostgreSQL.ReadModels;
using Domain.Entities.Categories;
using Domain.IRepositories.CategoryRepositories;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Infrastructure.Repositories.CategoryRepositories;

internal sealed class CategoryRepositoryCached : ICategoryRepository
{
    private readonly ICategoryRepository _decorated;
    private readonly IDistributedCache _cache;
    public CategoryRepositoryCached(
        ICategoryRepository decorated, 
        IDistributedCache cache)
    {
        _decorated = decorated;
        _cache = cache;
    }

    public async Task AddCategoryToMySQL(Category category)
    {
        await _decorated.AddCategoryToMySQL(category);
    }

    public async Task AddCategoryToPosgreSQL(Category category)
    {
        await _decorated.AddCategoryToPosgreSQL(category);
    }

    public async Task DeleteCategoryFromMySQL(CategoryId categoryId)
    {
        await _decorated.DeleteCategoryFromMySQL(categoryId);
    }

    public async Task DeleteCategoryFromPosgreSQL(CategoryId categoryId)
    {
        await _decorated.DeleteCategoryFromPosgreSQL(categoryId);
    }

    public async Task<Category?> GetCategoryById(CategoryId categoryId, CancellationToken cancellationToken = default)
    {
        return await _decorated.GetCategoryById(categoryId, cancellationToken);
    }

    public async Task<CategoryReadModel?> GetCategoryByIdCached(
        CategoryId categoryId, 
        CancellationToken cancellationToken = default)
    {
        string cacheKey = $"Category:{categoryId.Value}";
        string? cachedCategory = await _cache.GetStringAsync(cacheKey, cancellationToken);

        CategoryReadModel? category;
        if (string.IsNullOrEmpty(cachedCategory))
        {
            category = await _decorated.GetCategoryByIdCached(categoryId, cancellationToken);
            if (category is null)
            {
                return category;
            }

            await _cache.SetStringAsync(
                cacheKey,
                JsonConvert.SerializeObject(category),
                cancellationToken);

            return category;
        }

        category = JsonConvert.DeserializeObject<CategoryReadModel>(cachedCategory);

        return category;
    }

    public async Task<bool> IsCategoryNameExist(string categoryName, CancellationToken cancellationToken = default)
    {
        return await _decorated.IsCategoryNameExist(categoryName, cancellationToken);
    }
}
