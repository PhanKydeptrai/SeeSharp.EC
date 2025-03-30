using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Domain.Entities.Customers;
using Domain.Entities.Products;
using Domain.Entities.WishItems;
using Domain.IRepositories;
using Domain.IRepositories.WishItems;
using Domain.OutboxMessages.Services;
using SharedKernel;

namespace Application.Features.WishItemFeature.Commands;

internal sealed class AddWishListCommandHandler : ICommandHandler<AddWishListCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWishItemRepository _wishItemRepository;
    private readonly IOutBoxMessageServices _outboxMessageServices;
    private readonly IEventBus _eventBus;
    public AddWishListCommandHandler(
        IUnitOfWork unitOfWork,
        IWishItemRepository wishItemRepository,
        IOutBoxMessageServices outboxMessageServices,
        IEventBus eventBus)
    {
        _unitOfWork = unitOfWork;
        _wishItemRepository = wishItemRepository;
        _outboxMessageServices = outboxMessageServices;
        _eventBus = eventBus;
    }

    public async Task<Result> Handle(AddWishListCommand request, CancellationToken cancellationToken)
    {
        
        var wishItem = WishItem.NewWishItem(
            CustomerId.FromGuid(request.CustomerId),
            ProductId.FromGuid(request.ProductId));
        
        await _wishItemRepository.AddWishItemToPostgreSQL(wishItem);
        await _unitOfWork.SaveChangeAsync();
        return Result.Success();
    }
}
