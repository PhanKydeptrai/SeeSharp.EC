using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Application.Features.OrderFeature.Commands.AddProductToOrder;
using Application.IServices;
using Application.Outbox;
using Domain.Entities.Customers;
using Domain.Entities.OrderDetails;
using Domain.Entities.Orders;
using Domain.Entities.Products;
using Domain.IRepositories;
using Domain.IRepositories.Orders;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Errors;
using Domain.Utilities.Events.OrderEvents;
using SharedKernel;

namespace Application.Features.OrderFeature.Commands;

internal sealed class AddProductToOrderCommandHandler : ICommandHandler<AddProductToOrderCommand>
{

    #region Dependencies
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductQueryServices _productQueryServices;
    private readonly IOrderRepository _orderRepository;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IEventBus _eventBus;
    public AddProductToOrderCommandHandler(
        IUnitOfWork unitOfWork,
        IEventBus eventBus,
        IOrderRepository orderRepository,
        IProductQueryServices productQueryServices,
        IOutBoxMessageServices outBoxMessageServices)
    {
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
        _orderRepository = orderRepository;
        _productQueryServices = productQueryServices;
        _outBoxMessageServices = outBoxMessageServices;
    }
    #endregion

    public async Task<Result> Handle(AddProductToOrderCommand request, CancellationToken cancellationToken)
    {

        var order = await _orderRepository.GetOrderByCustomerIdFromPostgreSQL(CustomerId.FromGuid(request.CustomerId));

        ProductId productId = ProductId.FromGuid(request.ProductId);
        var productPrice = await _productQueryServices.GetAvailableProductPrice(
            ProductId.FromGuid(request.ProductId));

        if (productPrice is null) return Result.Failure(ProductError.NotFound(productId));

        if (order is not null) //* Order is exist
        {
            var orderDetail = await _orderRepository.CheckProductAvailabilityInOrder(order.OrderId, productId);
            if (orderDetail is not null) //* Order detail is exist
            {
                var orderTotal = order.Total.Value - orderDetail.UnitPrice.Value;

                // Update quantity and unit price of order detail
                orderDetail.UpdateQuantityAndProductPriceAfterAddMoreProduct(
                    OrderDetailQuantity.NewOrderDetailQuantity(request.Quantity),
                    productPrice!);

                // Update order total 
                orderTotal = orderTotal + orderDetail.UnitPrice.Value;
                order.ReplaceOrderTotal(OrderTotal.FromDecimal(orderTotal));

                //Publish event 0
                var message = new CustomerAddProductToOrderEvent(
                    order.OrderId,
                    orderDetail.OrderDetailId,
                    order.CustomerId.Value,
                    productId,
                    orderDetail.Quantity.Value,
                    orderDetail.UnitPrice.Value,
                    orderTotal,
                    Ulid.NewUlid().ToGuid(),
                    OrderMessageType.UpdateOrderDetailAndUpdateOrder);
                await OutboxMessageExtentions.InsertOutboxMessageAsync(
                    message.MessageId, message, 
                    _outBoxMessageServices);
                await _unitOfWork.SaveChangeAsync();

                await _eventBus.PublishAsync(message);
                return Result.Success();
            }
            else
            {
                //* Order detail is not exist
                var newOrderDetail = OrderDetail.NewOrderDetail(
                   order.OrderId,
                   ProductId.FromGuid(request.ProductId),
                   OrderDetailQuantity.NewOrderDetailQuantity(request.Quantity),
                   productPrice!);

                order.AddNewValueToOrderTotal(newOrderDetail.UnitPrice);
                await _orderRepository.AddNewOrderDetailToPostgreSQL(newOrderDetail);
                //Publish event 1
                var message = new CustomerAddProductToOrderEvent(
                    order.OrderId,
                    newOrderDetail.OrderDetailId,
                    order.CustomerId.Value,
                    productId,
                    newOrderDetail.Quantity.Value,
                    newOrderDetail.UnitPrice.Value,
                    order.Total.Value,
                    Ulid.NewUlid().ToGuid(),
                    OrderMessageType.CreateOrderDetailAndUpdateOrder);
                await OutboxMessageExtentions.InsertOutboxMessageAsync(
                    message.MessageId, message, 
                    _outBoxMessageServices);
                await _unitOfWork.SaveChangeAsync();
                await _eventBus.PublishAsync(message);
                return Result.Success();
            }
        }
        else
        {
            //* Order is not exist
            var newOrder = Order.NewOrder(
                CustomerId.FromGuid(request.CustomerId),
                OrderTotal.FromDecimal(productPrice!.Value * request.Quantity),
                OrderPaymentStatus.Waiting,
                OrderStatus.Waiting);

            var newOrderDetail = OrderDetail.NewOrderDetail(
                newOrder.OrderId,
                ProductId.FromGuid(request.ProductId),
                OrderDetailQuantity.NewOrderDetailQuantity(request.Quantity),
                productPrice!);

            await _orderRepository.AddNewOrderToPostgreSQL(newOrder);

            await _orderRepository.AddNewOrderDetailToPostgreSQL(newOrderDetail);

            //Publish event 2
            var message = new CustomerAddProductToOrderEvent(
                newOrder.OrderId,
                newOrderDetail.OrderDetailId,
                newOrder.CustomerId.Value,
                productId,
                newOrderDetail.Quantity.Value,
                newOrderDetail.UnitPrice.Value,
                newOrder.Total.Value,
                Ulid.NewUlid().ToGuid(),
                OrderMessageType.CreateAll);

            await OutboxMessageExtentions.InsertOutboxMessageAsync(message.MessageId, message, _outBoxMessageServices);
            await _unitOfWork.SaveChangeAsync();
            await _eventBus.PublishAsync(message);
            return Result.Success();
        }
    }

}
