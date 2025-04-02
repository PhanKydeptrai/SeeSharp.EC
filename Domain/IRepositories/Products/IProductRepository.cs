using Domain.Entities.Categories;
using Domain.Entities.Products;
using Domain.Entities.ProductVariants;

namespace Domain.IRepositories.Products;

public interface IProductRepository
{
    /// <summary>
    /// Create new product
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    Task AddProduct(Product product);

    /// <summary>
    /// Create new product variant
    /// </summary>
    /// <param name="productVariant"></param>
    /// <returns></returns>
    Task AddProductVariant(ProductVariant productVariant);

    /// <summary>
    /// Get product by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Product?> GetProduct(ProductId id);
    /// <summary>
    /// Xoá sản phẩm trong danh mục
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteProductByCategory(CategoryId id);
    
    /// <summary>
    /// Xoá variant của sản phẩm theo id sản phẩm
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteProductVariantByProduct(ProductId id);
    
    /// <summary>
    /// Khôi phục variant theo id sản phẩm
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task RestoreProductVariantByProduct(ProductId id);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task RestoreProductByCategory(CategoryId id);

    /// <summary>
    /// Xoá variant của sản phẩm theo id danh mục
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteProductVariantByCategory(CategoryId id);
    
    /// <summary>
    /// Khôi phục variant theo id danh mục
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task RestoreProductVariantByCategory(CategoryId id);
}
