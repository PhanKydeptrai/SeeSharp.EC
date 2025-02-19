using Domain.Entities.Categories;
using Domain.IRepositories.CategoryRepositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.MySQL;
using Persistence.Database.PostgreSQL;

namespace Persistence.Repositories.CategoryRepositories;

internal sealed class CategoryRepository : ICategoryRepository
{
    private readonly NextSharpMySQLWriteDbContext _mySQLDbContext;
    private readonly NextSharpPostgreSQLWriteDbContext _postgreSQLWriteDbContext;

    public CategoryRepository(
        NextSharpMySQLWriteDbContext mySQLDbContext,
        NextSharpPostgreSQLWriteDbContext postgreSQLWriteDbContext)
    {
        _mySQLDbContext = mySQLDbContext;
        _postgreSQLWriteDbContext = postgreSQLWriteDbContext;
    }

    public async Task AddCategoryToMySQL(Category category)
    {
        await _mySQLDbContext.Categories.AddAsync(category);
    }

    public async Task AddCategoryToPosgreSQL(Category category)
    {
        await _postgreSQLWriteDbContext.Categories.AddAsync(category);
    }

    public async Task<int> DeleteCategoryFromMySQL(CategoryId categoryId)
    {
        // Cập nhật trạng thái của Category
        return await _mySQLDbContext.Categories
            .Where(a => a.CategoryId == categoryId)
            .ExecuteUpdateAsync(
                a => a.SetProperty(
                    a => a.CategoryStatus,
                    CategoryStatus.Unavailable));
    }

    public async Task<int> DeleteCategoryFromPosgreSQL(CategoryId categoryId)
    {
        // Cập nhật trạng thái của Category
        return await _postgreSQLWriteDbContext.Categories
            .Where(a => a.CategoryId == categoryId)
            .ExecuteUpdateAsync(
                a => a.SetProperty(
                    a => a.CategoryStatus,
                    CategoryStatus.Unavailable));
    }

    public async Task<Category?> GetCategoryByIdFromMySQL(
        CategoryId categoryId,
        CancellationToken cancellationToken = default)
    {
        return await _mySQLDbContext.Categories.FindAsync(categoryId);
    }

    public async Task<Category?> GetCategoryByIdFromPostgreSQL(
        CategoryId categoryId,
        CancellationToken cancellationToken = default)
    {
        return await _postgreSQLWriteDbContext.Categories.FindAsync(categoryId);
    }

    public async Task<bool> IsCategoryNameExist(
        CategoryName categoryName,
        CancellationToken cancellationToken = default)
    {
        return await _mySQLDbContext.Categories
            .AsNoTracking()
            .AnyAsync(
                a => a.CategoryName == categoryName,
                cancellationToken);
    }

    public Task<bool> IsCategoryNameExistWhenUpdate(
        CategoryId categoryId, 
        CategoryName categoryName, 
        CancellationToken cancellationToken = default)
    {
        return _mySQLDbContext.Categories
            .AsNoTracking()
            .AnyAsync(
                a => a.CategoryName == categoryName 
                && a.CategoryId != categoryId, 
                cancellationToken);
        
    }

    public async Task<int> UpdateCategoryToMySQL(Category category)
    {
        return await _mySQLDbContext.Categories
            .Where(a => a.CategoryId == category.CategoryId)
            .ExecuteUpdateAsync(
                a => a.SetProperty(
                    a => a.CategoryName,
                    category.CategoryName)
                    .SetProperty(
                        a => a.ImageUrl,
                        category.ImageUrl));
    }

    public async Task<int> UpdateCategoryToPosgreSQL(Category category)
    {
        return await _postgreSQLWriteDbContext.Categories
            .Where(a => a.CategoryId == category.CategoryId)
            .ExecuteUpdateAsync(
                a => a.SetProperty(
                    a => a.CategoryName,
                    category.CategoryName)
                    .SetProperty(
                        a => a.ImageUrl,
                        category.ImageUrl));
    }
}