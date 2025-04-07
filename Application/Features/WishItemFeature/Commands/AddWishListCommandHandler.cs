using Application.Abstractions.Messaging;
using Domain.Entities.Customers;
using Domain.Entities.ProductVariants;
using Domain.Entities.WishItems;
using Domain.IRepositories;
using Domain.IRepositories.WishItems;
using SharedKernel;

namespace Application.Features.WishItemFeature.Commands;

internal sealed class AddWishListCommandHandler : ICommandHandler<AddWishListCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWishItemRepository _wishItemRepository;
    public AddWishListCommandHandler(
        IUnitOfWork unitOfWork,
        IWishItemRepository wishItemRepository)
    {
        _unitOfWork = unitOfWork;
        _wishItemRepository = wishItemRepository;
    }

    public async Task<Result> Handle(AddWishListCommand request, CancellationToken cancellationToken)
    {
        
        var wishItem = WishItem.NewWishItem(
            CustomerId.FromGuid(request.CustomerId),
            ProductVariantId.FromGuid(request.ProductVariantId));
        
        await _wishItemRepository.AddWishItemToPostgreSQL(wishItem);
        await _unitOfWork.SaveChangeAsync();
        return Result.Success();
    }
}
