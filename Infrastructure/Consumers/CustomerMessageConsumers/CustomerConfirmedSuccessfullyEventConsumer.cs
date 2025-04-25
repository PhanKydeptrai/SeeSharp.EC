using Domain.IRepositories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.CustomerEvents;
using FluentEmail.Core;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.CustomerMessageConsumers;
/// <summary>
/// Consumer gửi mail thông báo xác nhận tài khoản thành công cho khách hàng
/// </summary>
internal sealed class CustomerConfirmedSuccessfullyEventConsumer : IConsumer<CustomerConfirmedSuccessfullyEvent>
{
    private readonly ILogger<CustomerConfirmedSuccessfullyEventConsumer> _logger;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IFluentEmail _fluentEmail;
    private readonly IUnitOfWork _unitOfWork;

    public CustomerConfirmedSuccessfullyEventConsumer(
        ILogger<CustomerConfirmedSuccessfullyEventConsumer> logger,
        IOutBoxMessageServices outBoxMessageServices,
        IUnitOfWork unitOfWork,
        IFluentEmail fluentEmail)
    {
        _logger = logger;
        _outBoxMessageServices = outBoxMessageServices;
        _unitOfWork = unitOfWork;
        _fluentEmail = fluentEmail;
    }

    public async Task Consume(ConsumeContext<CustomerConfirmedSuccessfullyEvent> context)
    {
        var message = context.Message;

        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming CustomerConfirmedSuccessfullyEvent for email: {email}",
            message.Email);
        //--------------------------------------------------------------

        try
        {
            //Consume message            
            var email = _fluentEmail
                .To(message.Email)
                .Subject("Xác nhận tài khoản thành công")
                .Body($"<h1>Xin chào</h1><p>Tài khoản của bạn đã được xác nhận thành công. <br> Nhằm tăng trải nghiệm mua hàng của quý khách cửa hàng xin phép gửi tặng quý khách một mã giảm giá áp dụng trị giá 10K, quý khách có thể áp dụng ngay cho đơn hàng đầu tiên <br> Chúc quý khách mua hàng vui vẻ!</p>", isHtml: true);

            await email.SendAsync();

            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                context.Message.MessageId,
                OutboxMessageStatus.Processed,
                string.Empty,
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();

            //---------------------------------------------------------------
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                context.Message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume CustomerConfirmedSuccessfullyEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume CustomerConfirmedSuccessfullyEvent for email: {email}",
                message.Email);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed CustomerConfirmedSuccessfullyEvent for email: {email}",
            message.Email);
        //-------------------------------------------------
    }
}
