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

internal sealed class CustomerUpdateOrderDetailMessageConsumer : IConsumer<CustomerUpdateOrderDetailEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CustomerUpdateOrderDetailMessageConsumer> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    public CustomerUpdateOrderDetailMessageConsumer(
        IUnitOfWork unitOfWork,
        ILogger<CustomerUpdateOrderDetailMessageConsumer> logger,
        IOrderRepository orderRepository,
        IOutBoxMessageServices outBoxMessageServices)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _orderRepository = orderRepository;
        _outBoxMessageServices = outBoxMessageServices;
    }

    public async Task Consume(ConsumeContext<CustomerUpdateOrderDetailEvent> context)
    {
        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming CustomerUpdateOrderDetailEvent for OrderdetailId: {OrderdetailId}",
            context.Message.OrderDetailId);
        //--------------------------------------------------------------

        try
        {
            //Consume message            
            var orderDetail = await _orderRepository.GetOrderDetailByIdFromMySQL(
                OrderDetailId.FromGuid(context.Message.OrderDetailId));

            orderDetail!.Order!.UpdateOrderTotal(OrderTotal.NewOrderTotal(context.Message.OrderTotal));
            orderDetail!.ReplaceUnitPrice(OrderDetailUnitPrice.FromDecimal(context.Message.OrderDetailUnitPrice));
            await _unitOfWork.SaveToPostgreSQL();
            //---------------------------------------------------------------
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                context.Message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume CustomerUpdateOrderDetailEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveToMySQL();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume CustomerUpdateOrderDetailEvent for OrderdetailId: {OrderdetailId}",
                context.Message.OrderDetailId);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Cập nhật trạng thái hoàn thành outbox message -> Commit --
        await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
            context.Message.MessageId,
            OutboxMessageStatus.Processed,
            "Successfully consumed CustomerUpdateOrderDetailEvent",
            DateTime.UtcNow);

        await _unitOfWork.SaveToMySQL();
        //----------------------------------------------------------

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed CustomerUpdateOrderDetailEvent for OrderdetailId: {OrderdetailId}",
            context.Message.OrderDetailId);
        //-------------------------------------------------
    }
}
