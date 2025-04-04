using Application.DTOs.Product;
using Application.Features.Pages;
using Domain.Entities.Products;
using Domain.Entities.ProductVariants;

namespace Application.IServices;

public interface IProductQueryServices
{
    /// <summary>
    /// Get product price by product id
    /// </summary>
    /// <param name="productVariantId"></param>
    /// <returns></returns>
    Task<ProductVariantPrice?> GetAvailableProductPrice(ProductVariantId productVariantId);

    /// <summary>
    /// Get product variant by product variant id
    /// </summary>
    /// <param name="productVariantId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ProductVariantResponse?> GetVariantById(ProductVariantId productVariantId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get product by product id
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ProductResponse?> GetProductWithVariantListById(ProductId productId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all product with variant list
    /// </summary>
    /// <param name="filterProductStatus"></param>
    /// <param name="filterCategory"></param>
    /// <param name="searchTerm"></param>
    /// <param name="sortColumn"></param>
    /// <param name="sortOrder"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<PagedList<ProductResponse>> GetAllProductWithVariantList(
        string? filterProductStatus,
        string? filterCategory,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize);
    
    /// <summary>
    /// Get all product variant
    /// </summary>
    /// <param name="filterProductStatus"></param>
    /// <param name="filterCategory"></param>
    /// <param name="searchTerm"></param>
    /// <param name="sortColumn"></param>
    /// <param name="sortOrder"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<PagedList<ProductVariantResponse>> GetAllVariant(
        string? filterProductStatus,
        string? filterProduct,
        string? filterCategory,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize);
    

    /// <summary>
    /// Check if product name is exist
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="productName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsProductNameExist(
        ProductId? productId,
        ProductName productName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if product variant name is exist
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="productVariantName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsProductVariantNameExist(
        ProductId productId,
        ProductVariantId? productVariantId,
        VariantName productVariantName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if product can be sold
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    Task<bool> CheckProductAvailability(ProductId productId);
    /// <summary>
    /// Check if product is exist (Used for checking product before adding to wish list)
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    Task<bool> IsProductExist(ProductId productId);

    /// <summary>
    /// Check if product variant is exist (Used for checking product before adding to wish list)
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    Task<bool> IsProductVariantExist(ProductVariantId productId);
}
