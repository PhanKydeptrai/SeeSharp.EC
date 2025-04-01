using Domain.Entities.Categories;
using Domain.Entities.Products;
using Domain.Entities.ProductVariants;

namespace Domain.IRepositories.Products;

public interface IProductRepository
{
    #region 🐘 PostgreSQL
    /// <summary>
    /// Create new product
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    Task AddProductToPostgreSQL(Product product);

    /// <summary>
    /// Create new product variant
    /// </summary>
    /// <param name="productVariant"></param>
    /// <returns></returns>
    Task AddProductVariantToPostgreSQL(ProductVariant productVariant);

    /// <summary>
    /// Get product by id
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
