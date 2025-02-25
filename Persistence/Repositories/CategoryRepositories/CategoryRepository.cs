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

    public async Task<bool> IsCategoryIdExist(CategoryId categoryId, CancellationToken cancellationToken = default)
    {
        return await _mySQLDbContext.Categories
            .AsNoTracking()
            .AnyAsync(
            a => a.CategoryId == categoryId 
            && a.CategoryStatus != CategoryStatus.Deleted, 
            cancellationToken);
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

    public async Task<bool> IsCategoryNameExistWhenUpdate(
        CategoryId categoryId, 
        CategoryName categoryName, 
        CancellationToken cancellationToken = default)
    {
        return await _mySQLDbContext.Categories
            .AsNoTracking()
            .AnyAsync(
                a => a.CategoryName == categoryName 
                && a.CategoryId != categoryId, 
                cancellationToken);
        
    }
}