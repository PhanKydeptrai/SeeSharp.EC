using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Domain.IRepositories;
using Domain.IRepositories.WishItems;
using Domain.OutboxMessages.Services;
using SharedKernel;

namespace Application.Features.WishItemFeature.Commands.DeleteWishItemFromWishList;

internal sealed class DeleteWishItemFromWishListCommandHandler : ICommandHandler<DeleteWishItemFromWishListCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWishItemRepository _wishItemRepository;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IEventBus _eventBus;
    public DeleteWishItemFromWishListCommandHandler(
        IUnitOfWork unitOfWork,
        IWishItemRepository wishItemRepository,
        IOutBoxMessageServices outBoxMessageServices,
        IEventBus eventBus)
    {
        _unitOfWork = unitOfWork;
        _wishItemRepository = wishItemRepository;
        _outBoxMessageServices = outBoxMessageServices;
        _eventBus = eventBus;
    }

    public async Task<Result> Handle(
        DeleteWishItemFromWishListCommand request, 
        CancellationToken cancellationToken)
    {
        
        throw new NotImplementedException();
    }

}
