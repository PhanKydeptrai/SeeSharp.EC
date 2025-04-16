using Application.Abstractions.Messaging;
using Application.DTOs.WishItems;
using Application.Features.Pages;
using Application.IServices;
using Domain.Entities.Customers;
using SharedKernel;

namespace Application.Features.WishItemFeature.Queries.GetWishList;

internal sealed class GetWishListQueryHandler : IQueryHandler<GetWishListQuery, PagedList<WishlistResponse>>
{
    private readonly IWishItemQueryServices _wishItemQueryServices;

    public GetWishListQueryHandler(IWishItemQueryServices wishItemQueryServices)
    {
        _wishItemQueryServices = wishItemQueryServices;
    }

    public async Task<Result<PagedList<WishlistResponse>>> Handle(GetWishListQuery request, CancellationToken cancellationToken)
    {
        var result = await _wishItemQueryServices.GetWishList(
            request.productStatusFilter,
            request.searchTerm,
            request.sortColumn,
            request.sortOrder,
            request.page,
            request.pageSize,
            CustomerId.FromGuid(request.CustomerId)
        );

        return Result.Success(result);
    }
}
