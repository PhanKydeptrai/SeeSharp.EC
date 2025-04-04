using Application.Abstractions.Messaging;
using Application.DTOs.WishItems;
using Application.Features.Pages;
using Application.IServices;
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
        throw new NotImplementedException();
    }
}
