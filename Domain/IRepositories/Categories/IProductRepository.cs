using Domain.Entities.Categories;
using Domain.Entities.Products;

namespace Domain.IRepositories.Categories;

public interface IProductRepository
{
    Task AddProductToMySQL(Product product);
    Task AddProductToPostgreSQL(Product product);
    Task<Product?> GetProductFromMySQL(ProductId id);
    Task<Product?> GetProductFromPostgreSQL(ProductId id);
    #region List of products by category id
    Task DeleteProductByCategoryFromMySQL(CategoryId id);
    Task DeleteProductByCategoryFromPosgreSQL(CategoryId id);
    Task RestoreProductByCategoryFromMySQL(CategoryId id);
    Task RestoreProductByCategoryFromPostgreSQL(CategoryId id);
    #endregion

}
