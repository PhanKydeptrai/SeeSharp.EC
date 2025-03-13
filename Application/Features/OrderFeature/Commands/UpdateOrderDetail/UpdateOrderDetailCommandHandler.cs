using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Application.IServices;
using Application.Outbox;
using Domain.Entities.OrderDetails;
using Domain.Entities.Orders;
using Domain.IRepositories;
using Domain.IRepositories.Orders;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Errors;
using Domain.Utilities.Events.OrderEvents;
using SharedKernel;

namespace Application.Features.OrderFeature.Commands.UpdateOrderDetail;

internal sealed class UpdateOrderDetailCommandHandler : ICommandHandler<UpdateOrderDetailCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductQueryServices _productQueryServices;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IEventBus _eventBus;
    public UpdateOrderDetailCommandHandler(
        IUnitOfWork unitOfWork,
        IOrderRepository orderRepository,
        IProductQueryServices productQueryServices,
        IOutBoxMessageServices outBoxMessageServices,
        IEventBus eventBus)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _productQueryServices = productQueryServices;
        _outBoxMessageServices = outBoxMessageServices;
        _eventBus = eventBus;
    }

    public async Task<Result> Handle(UpdateOrderDetailCommand request, CancellationToken cancellationToken)
    {
        OrderDetailId orderDetailId = OrderDetailId.FromGuid(request.OrderDetailId);

        var orderDetail = await _orderRepository.GetOrderDetailByIdFromMySQL(orderDetailId);

        if (orderDetail is null) return Result.Failure(OrderError.NotFoundOrderDetail(orderDetailId));
        //get product price
        var productPrice = await _productQueryServices.GetAvailableProductPrice(orderDetail.ProductId);

        // Update quantity and unit price of order detail
        orderDetail.UpdateQuantityProductPrice(
            OrderDetailQuantity.NewOrderDetailQuantity(request.Quantity),
            productPrice!);

        //update order total
        var orderTotal = orderDetail.Order!.Total.Value - orderDetail.UnitPrice.Value;
        orderDetail.Order.UpdateOrderTotal(OrderTotal.FromDecimal(orderTotal));
        
        var message = new CustomerUpdateOrderDetailEvent(
            orderDetail.OrderDetailId,
            orderDetail.Quantity.Value,
            orderDetail.UnitPrice.Value,
            orderDetail.Order.Total.Value,
            Ulid.NewUlid().ToGuid());

        await OutboxMessageExtentions.InsertOutboxMessageAsync(message.MessageId, message, _outBoxMessageServices);
        await _unitOfWork.SaveToMySQL();
        
        await _eventBus.PublishAsync(message);
        return Result.Success();
    }
}
