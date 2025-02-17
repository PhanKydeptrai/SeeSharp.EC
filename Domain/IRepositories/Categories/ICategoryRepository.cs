using Domain.Database.PostgreSQL.ReadModels;
using Domain.Entities.Categories;

namespace Domain.IRepositories.CategoryRepositories;

public interface ICategoryRepository
{
    #region Write
    Task AddCategoryToMySQL(Category category);
    Task AddCategoryToPosgreSQL(Category category);
    Task DeleteCategoryFromMySQL(CategoryId categoryId);
    Task DeleteCategoryFromPosgreSQL(CategoryId categoryId);
    #endregion

    #region Validation
    Task<bool> IsCategoryNameExist(string categoryName,
        CancellationToken cancellationToken = default);
    #endregion

    #region Read
    Task<Category?> GetCategoryById(CategoryId categoryId,
        CancellationToken cancellationToken = default); //Without cache
    Task<CategoryReadModel?> GetCategoryByIdCached(CategoryId categoryId,
        CancellationToken cancellationToken = default); //With cache
    #endregion
}
