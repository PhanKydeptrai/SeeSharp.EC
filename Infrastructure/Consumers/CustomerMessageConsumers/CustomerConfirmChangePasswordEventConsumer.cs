using Domain.IRepositories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.CustomerEvents;
using FluentEmail.Core;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.CustomerMessageConsumers;

/// <summary>
/// Consumer gửi mail thông báo xác nhận thay đổi mật khẩu thành công cho khách hàng
/// </summary>
internal sealed class CustomerConfirmChangePasswordEventConsumer : IConsumer<CustomerConfirmChangePasswordEvent>
{
    private readonly ILogger<CustomerConfirmChangePasswordEventConsumer> _logger;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IFluentEmail _fluentEmail;
    private readonly IUnitOfWork _unitOfWork;

    public CustomerConfirmChangePasswordEventConsumer(
        ILogger<CustomerConfirmChangePasswordEventConsumer> logger,
        IOutBoxMessageServices outBoxMessageServices,
        IUnitOfWork unitOfWork,
        IFluentEmail fluentEmail)
    {
        _logger = logger;
        _outBoxMessageServices = outBoxMessageServices;
        _unitOfWork = unitOfWork;
        _fluentEmail = fluentEmail;
    }

    public async Task Consume(ConsumeContext<CustomerConfirmChangePasswordEvent> context)
    {
        var message = context.Message;

        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming CustomerConfirmChangePasswordEvent for email: {email}",
            message.Email);
        //--------------------------------------------------------------

        try
        {
            //Consume message            
            var email = _fluentEmail
                .To(message.Email)
                .Subject("Xác nhận thay đổi mật khẩu thành công")
                .Body($"<h1>Xin chào</h1><p>Mật khẩu của bạn đã được thay đổi thành công. Nếu bạn không thực hiện thay đổi này, vui lòng liên hệ với chúng tôi ngay lập tức.</p>", isHtml: true);

            await email.SendAsync();

            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                message.MessageId,
                OutboxMessageStatus.Processed,
                string.Empty,
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume CustomerConfirmChangePasswordEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume CustomerConfirmChangePasswordEvent for email: {email}",
                message.Email);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed CustomerConfirmChangePasswordEvent for email: {email}",
            message.Email);
        //-------------------------------------------------
    }
} 