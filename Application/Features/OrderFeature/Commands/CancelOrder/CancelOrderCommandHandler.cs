using Application.Abstractions.Messaging;
using Domain.Entities.Orders;
using Domain.IRepositories;
using Domain.IRepositories.Orders;
using SharedKernel;

namespace Application.Features.OrderFeature.Commands.CancelOrder;

internal sealed class CancelOrderCommandHandler : ICommandHandler<CancelOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelOrderCommandHandler(
        IOrderRepository orderRepository, 
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var orderId = OrderId.FromGuid(request.OrderId);
        var order = await _orderRepository.GetOrderById(orderId);
        if (order is null)
        {
            return Result.Failure(new Error("OrderNotFound", "Order not found.", ErrorType.NotFound));
        }

        if(order.OrderStatus != OrderStatus.New)
        {
            return Result.Failure(new Error(
                "OrderNotInNew", 
                "Order is not in new status.", 
                ErrorType.Problem));
        }

        order.Cancel();
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}
