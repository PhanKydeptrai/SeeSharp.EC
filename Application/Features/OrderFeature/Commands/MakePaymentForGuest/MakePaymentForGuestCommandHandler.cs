using Application.Abstractions.Messaging;
using Domain.IRepositories;
using Domain.IRepositories.Orders;
using SharedKernel;

namespace Application.Features.OrderFeature.Commands.MakePaymentForGuest;

internal sealed class MakePaymentForGuestCommandHandler : ICommandHandler<MakePaymentForGuestCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;

    public MakePaymentForGuestCommandHandler(
        IUnitOfWork unitOfWork, 
        IOrderRepository orderRepository)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
    }

    public Task<Result> Handle(MakePaymentForGuestCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
