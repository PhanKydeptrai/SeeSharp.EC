using Domain.Entities.Customers;
using Domain.Entities.OrderDetails;
using Domain.Entities.Orders;
using Domain.Entities.Products;
using Domain.IRepositories;
using Domain.IRepositories.Orders;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.OrderEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.OrderMessageConsumer;

public sealed class CustomerAddProductToOrderMessageConsumer : IConsumer<CustomerAddProductToOrderEvent>
{
    private readonly ILogger<CustomerAddProductToOrderMessageConsumer> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IUnitOfWork _unitOfWork;
    public CustomerAddProductToOrderMessageConsumer(
        ILogger<CustomerAddProductToOrderMessageConsumer> logger,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        IOutBoxMessageServices outBoxMessageServices)
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _outBoxMessageServices = outBoxMessageServices;
    }

    public async Task Consume(ConsumeContext<CustomerAddProductToOrderEvent> context)
    {

        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming CustomerAddProductToOrderEvent for CustomerId: {CustomerId}",
            context.Message.CustomerId);
        //--------------------------------------------------------------

        try
        {
            //Consume message
            if (context.Message.MessageType == OrderMessageType.UpdateOrderDetailAndUpdateOrder)
            {
                var orderDetail = await _orderRepository.GetOrderDetailByIdFromPostgreSQL(
                    OrderDetailId.FromGuid(context.Message.OrderDetailId));

                var order = await _orderRepository.GetOrderByIdFromPostgreSQL(
                    OrderId.FromGuid(context.Message.OrderId));

                order!.ReplaceOrderTotal(OrderTotal.NewOrderTotal(context.Message.OrderTotal));
                orderDetail!.ReplaceUnitPrice(OrderDetailUnitPrice.NewOrderDetailUnitPrice(context.Message.OrderDetailUnitPrice));
                orderDetail!.ReplaceQuantity(OrderDetailQuantity.NewOrderDetailQuantity(context.Message.OrderDetailQuantity));
                await _unitOfWork.SaveToPostgreSQL();
                //----------------------------------------------------------
            }
            else if (context.Message.MessageType == OrderMessageType.CreateOrderDetailAndUpdateOrder)
            {
                var order = await _orderRepository.GetOrderByIdFromPostgreSQL(
                    OrderId.FromGuid(context.Message.OrderId));

                //Create new order detail
                var orderDetail = OrderDetail.FromExisting(
                    OrderDetailId.FromGuid(context.Message.OrderDetailId),
                    order!.OrderId,
                    ProductId.FromGuid(context.Message.ProductId),
                    OrderDetailQuantity.NewOrderDetailQuantity(context.Message.OrderDetailQuantity),
                    OrderDetailUnitPrice.FromDecimal(context.Message.OrderDetailUnitPrice));
                
                await _orderRepository.AddNewOrderDetailToPostgreSQL(orderDetail);
                await _unitOfWork.SaveToPostgreSQL();
            }
            else if(context.Message.MessageType == OrderMessageType.CreateAll)
            {
                var order = Order.FromExisting(
                    OrderId.FromGuid(context.Message.OrderId),
                    CustomerId.FromGuid(context.Message.CustomerId),
                    OrderTotal.NewOrderTotal(context.Message.OrderTotal),
                    OrderPaymentStatus.Waiting,
                    OrderStatus.Waiting);
                
                var orderDetail = OrderDetail.FromExisting(
                    OrderDetailId.FromGuid(context.Message.OrderDetailId),
                    order.OrderId,
                    ProductId.FromGuid(context.Message.ProductId),
                    OrderDetailQuantity.NewOrderDetailQuantity(context.Message.OrderDetailQuantity),
                    OrderDetailUnitPrice.FromDecimal(context.Message.OrderDetailUnitPrice));
                
                await _orderRepository.AddNewOrderToPostgreSQL(order);
                await _orderRepository.AddNewOrderDetailToPostgreSQL(orderDetail);
                await _unitOfWork.SaveToPostgreSQL();
            }
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                context.Message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume CustomerAddProductToOrderEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveToMySQL();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume CustomerAddProductToOrderEvent for CustomerId: {CustomerId}",
                context.Message.CustomerId);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Cập nhật trạng thái hoàn thành outbox message -> Commit --
        await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
            context.Message.MessageId,
            OutboxMessageStatus.Processed,
            "Successfully consumed CustomerAddProductToOrderEvent",
            DateTime.UtcNow);

        await _unitOfWork.SaveToMySQL();


        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed CustomerAddProductToOrderEvent for CustomerId: {CustomerId}",
            context.Message.CustomerId);
        //-------------------------------------------------
    }
}
