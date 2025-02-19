using Domain.Entities.Categories;

namespace Domain.IRepositories.CategoryRepositories;

//* No cache in this place ok?
public interface ICategoryRepository
{
    #region Write
    Task AddCategoryToMySQL(Category category);
    Task AddCategoryToPosgreSQL(Category category);
    Task DeleteCategoryFromMySQL(CategoryId categoryId);
    Task DeleteCategoryFromPosgreSQL(CategoryId categoryId);
    Task UpdateCategoryToMySQL(Category category);
    Task UpdateCategoryToPosgreSQL(Category category);
    #endregion

    #region Validation
    Task<bool> IsCategoryNameExist(string categoryName,
        CancellationToken cancellationToken = default);
    #endregion

    #region Read
    Task<Category?> GetCategoryByIdFromMySQL(CategoryId categoryId,
        CancellationToken cancellationToken = default);
    Task<Category?> GetCategoryByIdFromPostgreSQL(CategoryId categoryId,
    CancellationToken cancellationToken = default); 
    #endregion
}
