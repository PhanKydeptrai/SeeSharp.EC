using Domain.Database.PostgreSQL.ReadModels;
using Domain.Entities.Categories;
using Domain.IRepositories.CategoryRepositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.MySQL;
using Persistence.Database.PostgreSQL;

namespace Persistence.Repositories.CategoryRepositories;

internal sealed class CategoryRepository : ICategoryRepository
{
    private readonly NextSharpMySQLWriteDbContext _mySQLDbContext;
    private readonly NextSharpPostgreSQLReadDbContext _postgreSQLReadDbContext;
    private readonly NextSharpPostgreSQLWriteDbContext _postgreSQLWriteDbContext;

    public CategoryRepository(
        NextSharpMySQLWriteDbContext mySQLDbContext, 
        NextSharpPostgreSQLReadDbContext postgreSQLReadDbContext, 
        NextSharpPostgreSQLWriteDbContext postgreSQLWriteDbContext)
    {
        _mySQLDbContext = mySQLDbContext;
        _postgreSQLReadDbContext = postgreSQLReadDbContext;
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

    public async Task DeleteCategoryFromMySQL(CategoryId categoryId)
    {
        // Cập nhật trạng thái của Category
        await _mySQLDbContext.Categories
            .Where(a => a.CategoryId == categoryId)
            .ExecuteUpdateAsync(a => a.SetProperty(a => a.CategoryStatus, CategoryStatus.Unavailable));
    }

    public async Task DeleteCategoryFromPosgreSQL(CategoryId categoryId)
    {
        // Cập nhật trạng thái của Category
        await _postgreSQLWriteDbContext.Categories
            .Where(a => a.CategoryId == categoryId)
            .ExecuteUpdateAsync(a => a.SetProperty(a => a.CategoryStatus, CategoryStatus.Unavailable));
    }

    public async Task<Category?> GetCategoryById(CategoryId categoryId, //From MySQL
        CancellationToken cancellationToken = default)
    {
        return await _mySQLDbContext.Categories.FindAsync(categoryId);
    }

    public async Task<CategoryReadModel?> GetCategoryByIdCached(CategoryId categoryId, 
        CancellationToken cancellationToken = default)
    {
        return await _postgreSQLReadDbContext.Categories.FindAsync(categoryId.Value, cancellationToken);
    }

    public async Task<bool> IsCategoryNameExist(string categoryName, 
        CancellationToken cancellationToken = default)
    {
        return await _mySQLDbContext.Categories.AnyAsync(a => a.CategoryName.Value == categoryName, cancellationToken);
    }
}
