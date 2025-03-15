using Application.Abstractions.Messaging;
using Application.DTOs.WishItems;
using Application.Features.Pages;

namespace Application.Features.WishItemFeature.Queries.GetWishList;

public record GetWishListQuery(
    string? productStatusFilter,
    string? searchTerm,
    string? sortColumn,
    string? sortOrder,
    int? page,
    int? pageSize,
    Guid CustomerId) : IQuery<PagedList<WishlistResponse>>;
