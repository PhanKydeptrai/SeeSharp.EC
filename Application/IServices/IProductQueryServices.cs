using Application.DTOs.Product;
using Application.Features.Pages;
using Domain.Entities.Products;

namespace Application.IServices;

public interface IProductQueryServices
{
    /// <summary>
    /// Get product price by product id
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    Task<ProductPrice?> GetAvailableProductPrice(ProductId productId);
    /// <summary>
    /// Get product by product id
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ProductResponse?> GetById(ProductId productId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Get product by product name
    /// </summary>
    /// <param name="filterProductStatus"></param>
    /// <param name="filterCategory"></param>
    /// <param name="searchTerm"></param>
    /// <param name="sortColumn"></param>
    /// <param name="sortOrder"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<PagedList<ProductResponse>> PagedList(
        string? filterProductStatus,
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
    /// Check if product can be sold
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    Task<bool> CheckProductAvailability(ProductId productId);
}
