using Domain.Entities.Categories;

namespace Domain.IRepositories.CategoryRepositories;

//* No cache in this place ok?
public interface ICategoryRepository
{
    #region Write
    Task AddCategoryToMySQL(Category category);
    Task AddCategoryToPosgreSQL(Category category);
    Task UpdateCategoryToMySQL_ChangeTracking(
        CategoryId categoryId, 
        CategoryName categoryName,
        string imageUrl);
    Task UpdateCategoryPostgreSQL_ChangeTracking(
        CategoryId categoryId,
        CategoryName categoryName,
        string imageUrl);
    #endregion

    #region Validation 
    Task<bool> IsCategoryNameExist(CategoryName categoryName,
        CancellationToken cancellationToken = default);
    //Check if category name exists when update
   Task<bool> IsCategoryNameExistWhenUpdate(
        CategoryId categoryId, 
        CategoryName categoryName,
        CancellationToken cancellationToken = default);
    #endregion

    #region Read
    Task<Category?> GetCategoryByIdFromMySQL(CategoryId categoryId,
        CancellationToken cancellationToken = default);
    Task<Category?> GetCategoryByIdFromPostgreSQL(CategoryId categoryId,
    CancellationToken cancellationToken = default); 
    #endregion
}
