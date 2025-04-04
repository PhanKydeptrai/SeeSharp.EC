using Application.Abstractions.Messaging;
using Domain.Entities.WishItems;
using Domain.IRepositories;
using Domain.IRepositories.WishItems;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.WishItemFeature.Commands.DeleteWishItemFromWishList;

internal sealed class DeleteWishItemFromWishListCommandHandler : ICommandHandler<DeleteWishItemFromWishListCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWishItemRepository _wishItemRepository;
    public DeleteWishItemFromWishListCommandHandler(
        IUnitOfWork unitOfWork,
        IWishItemRepository wishItemRepository)
    {
        _unitOfWork = unitOfWork;
        _wishItemRepository = wishItemRepository;
    }

    public async Task<Result> Handle(
        DeleteWishItemFromWishListCommand request, 
        CancellationToken cancellationToken)
    {
        var wishItemId = WishItemId.FromGuid(request.WishItemId);
        var wishItem = await _wishItemRepository.GetWishItemByIdFromPostgreSQL(wishItemId);

        if (wishItem is null)
        {
            return Result.Failure(WishItemError.NotFound(wishItemId));
        }

        _wishItemRepository.DeleteWishItemFromPostgreSQL(wishItem);
        
        await _unitOfWork.SaveChangeAsync();

        return Result.Success();
    }

}
