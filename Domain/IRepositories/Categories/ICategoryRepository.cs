using Domain.Entities.Categories;

namespace Domain.IRepositories.CategoryRepositories;

//* No cache in this place ok?
public interface ICategoryRepository
{
    #region Write
    Task AddCategoryToMySQL(Category category);
    Task AddCategoryToPosgreSQL(Category category);        
    #endregion
    Task<bool> IsCategoryIdExist(CategoryId categoryId, CancellationToken cancellationToken = default);
    #region Read
    Task<Category?> GetCategoryByIdFromMySQL(CategoryId categoryId,
        CancellationToken cancellationToken = default);
    Task<Category?> GetCategoryByIdFromPostgreSQL(CategoryId categoryId,
    CancellationToken cancellationToken = default); 
    #endregion
}
