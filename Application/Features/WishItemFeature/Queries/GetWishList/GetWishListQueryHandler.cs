using Application.Abstractions.Messaging;
using Application.DTOs.WishItems;
using Application.Features.Pages;
using SharedKernel;

namespace Application.Features.WishItemFeature.Queries.GetWishList;

internal sealed class GetWishListQueryHandler : IQueryHandler<GetWishListQuery, PagedList<WishlistResponse>>
{
    public Task<Result<PagedList<WishlistResponse>>> Handle(GetWishListQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
