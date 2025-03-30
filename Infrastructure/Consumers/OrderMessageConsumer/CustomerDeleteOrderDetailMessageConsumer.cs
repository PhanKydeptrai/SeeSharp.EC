using Domain.Entities.OrderDetails;
using Domain.Entities.Orders;
using Domain.IRepositories;
using Domain.IRepositories.Orders;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.OrderEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.OrderMessageConsumer;

internal sealed class CustomerDeleteOrderDetailMessageConsumer : IConsumer<CustomerDeleteOrderDetailEvent>
{
    private readonly ILogger<CustomerDeleteOrderDetailMessageConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IOrderRepository _orderRepository;
    public CustomerDeleteOrderDetailMessageConsumer(
        ILogger<CustomerDeleteOrderDetailMessageConsumer> logger,
        IUnitOfWork unitOfWork,
        IOrderRepository orderRepository,
        IOutBoxMessageServices outBoxMessageServices)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _outBoxMessageServices = outBoxMessageServices;
    }

    public async Task Consume(ConsumeContext<CustomerDeleteOrderDetailEvent> context)
    {
        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming CustomerDeleteOrderDetailEvent for OrderDetailId: {OrderDetailId}",
            context.Message.OrderDetailId);
        //--------------------------------------------------------------

        try
        {
            //Consume message            
            var orderDetail = await _orderRepository.GetOrderDetailByIdFromPostgreSQL(
                OrderDetailId.FromGuid(context.Message.OrderDetailId));
            
            orderDetail!.Order!.ReplaceOrderTotal(OrderTotal.FromDecimal(context.Message.NewOrderTotal));
            _orderRepository.DeleteOrderDetailFromPostgeSQL(orderDetail);
            await _unitOfWork.SaveChangeAsync();
            //---------------------------------------------------------------
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                context.Message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume CustomerDeleteOrderDetailEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangeAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume CustomerDeleteOrderDetailEvent for OrderDetailId: {OrderDetailId}",
                context.Message.OrderDetailId);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Cập nhật trạng thái hoàn thành outbox message -> Commit --
        await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
            context.Message.MessageId,
            OutboxMessageStatus.Processed,
            "Successfully consumed CustomerDeleteOrderDetailEvent",
            DateTime.UtcNow);

        await _unitOfWork.SaveChangeAsync();
        //----------------------------------------------------------

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed CustomerDeleteOrderDetailEvent for OrderDetailId: {OrderDetailId}",
            context.Message.OrderDetailId);
        //-------------------------------------------------
    }
}
