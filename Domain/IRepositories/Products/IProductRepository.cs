using Domain.Entities.Categories;
using Domain.Entities.Products;

namespace Domain.IRepositories.Products;

public interface IProductRepository
{
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
