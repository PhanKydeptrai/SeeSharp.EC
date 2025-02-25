using Application.DTOs.Product;
using Application.Features.Pages;
using Domain.Entities.Products;

namespace Application.IServices;

public interface IProductQueryServices
{
    Task<ProductResponse?> GetById(ProductId productId, CancellationToken cancellationToken = default);
    Task<PagedList<ProductResponse>> PagedList(
        string? filterProductStatus,
        string? filterCategory,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize);
    Task<bool> IsProductNameExist(
        ProductId? productId, 
        ProductName productName, 
        CancellationToken cancellationToken = default);
}
