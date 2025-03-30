using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Application.Outbox;
using Domain.Entities.OrderDetails;
using Domain.Entities.Orders;
using Domain.IRepositories;
using Domain.IRepositories.Orders;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Errors;
using Domain.Utilities.Events.OrderEvents;
using SharedKernel;

namespace Application.Features.OrderFeature.Commands.DeleteOrderDetail;

internal sealed class DeleteOrderDetailCommandHandler : ICommandHandler<DeleteOrderDetailCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IEventBus _eventBus;

    public DeleteOrderDetailCommandHandler(
        IUnitOfWork unitOfWork,
        IOrderRepository orderRepository,
        IOutBoxMessageServices outBoxMessageServices,
        IEventBus eventBus)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _outBoxMessageServices = outBoxMessageServices;
        _eventBus = eventBus;
    }

    public async Task<Result> Handle(DeleteOrderDetailCommand request, CancellationToken cancellationToken)
    {
        OrderDetailId orderDetailId = OrderDetailId.FromGuid(request.OrderDetailId);
        var orderDetail = await _orderRepository.GetOrderDetailByIdFromPostgreSQL(orderDetailId);

        if (orderDetail is null)
        {
            return Result.Failure(OrderError.OrderDetailNotFound(orderDetailId));
        }
        //Update order total
        var newOrderTotal = OrderTotal.NewOrderTotal(orderDetail.Order!.Total.Value - orderDetail.UnitPrice.Value) ; 
        orderDetail.Order.ReplaceOrderTotal(newOrderTotal);
        // Delete order detail from MySQL
        _orderRepository.DeleteOrderDetailFromPostgeSQL(orderDetail);
        // Add new outbox message
        var message = new CustomerDeleteOrderDetailEvent(
            orderDetailId, 
            newOrderTotal.Value,
            Ulid.NewUlid().ToGuid());
        await OutboxMessageExtentions.InsertOutboxMessageAsync(message.MessageId, message, _outBoxMessageServices);
        
        await _unitOfWork.SaveChangeAsync();
        
        await _eventBus.PublishAsync(message);
        return Result.Success();
    }
}
