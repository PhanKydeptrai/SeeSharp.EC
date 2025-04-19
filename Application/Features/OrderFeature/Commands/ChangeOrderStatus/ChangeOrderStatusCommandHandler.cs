using Application.Abstractions.Messaging;
using Domain.Entities.Orders;
using Domain.IRepositories;
using Domain.IRepositories.Orders;
using SharedKernel;

namespace Application.Features.OrderFeature.Commands.ChangeOrderStatus;

internal sealed class ChangeOrderStatusCommandHandler : ICommandHandler<ChangeOrderStatusCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeOrderStatusCommandHandler(
        IOrderRepository orderRepository, 
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ChangeOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var orderId = OrderId.FromGuid(request.OrderId);
        var order = _orderRepository.GetOrderById(orderId).Result;
        if (order is null)
        {
            return Result.Failure(new Error("OrderNotFound", "Order not found.", ErrorType.NotFound));
        }

        if (order.OrderStatus == OrderStatus.New)
        {
            return Result.Failure(new Error(
                "OrderNotInNew", 
                "Order is not in new status.", 
                ErrorType.Problem));
        }

        if (order.OrderStatus == OrderStatus.Delivered)
        {
            return Result.Failure(new Error(
                "OrderAlreadyDelivered", 
                "Order is already delivered.", 
                ErrorType.Problem));
        }

        if (order.OrderStatus == OrderStatus.Processing)
        {
            order.ChangeOrderStatus(OrderStatus.Shipped);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success(); 
        }

        if (order.OrderStatus == OrderStatus.Shipped)
        {
            order.ChangeOrderStatus(OrderStatus.Delivered);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }


        // if (order.OrderStatus == OrderStatus.Delivered)
        // {
        //     return Result.Failure(new Error(
        //         "OrderShipped", 
        //         "Order is shipped.", 
        //         ErrorType.Problem));
        // }

        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}
