using Domain.Entities.Categories;
using Domain.Entities.Products;

namespace Domain.IRepositories.Products;

public interface IProductRepository
{
    #region 🐬 MySQL 
    /// <summary>
    /// Add product to MySQL
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    Task AddProductToMySQL(Product product);
    /// <summary>
    /// Get product from MySQL
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Product?> GetProductFromMySQL(ProductId id);
    /// <summary>
    /// Delete all products in this category => Set category id to default category id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteProductByCategoryFromMySQL(CategoryId id);
    Task RestoreProductByCategoryFromMySQL(CategoryId id);
    #endregion

    #region 🐘 PostgreSQL
    /// <summary>
    /// 
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    Task AddProductToPostgreSQL(Product product);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Product?> GetProductFromPostgreSQL(ProductId id);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteProductByCategoryFromPosgreSQL(CategoryId id);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task RestoreProductByCategoryFromPostgreSQL(CategoryId id);
    #endregion
}
