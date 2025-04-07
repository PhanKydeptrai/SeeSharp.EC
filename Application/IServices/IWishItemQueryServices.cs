using Application.DTOs.WishItems;
using Application.Features.Pages;
using Domain.Entities.Customers;

namespace Application.IServices;

public interface IWishItemQueryServices
{
    Task<PagedList<WishlistResponse>> GetWishList(
        string? productStatus,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize,
        CustomerId customerId); 
}
