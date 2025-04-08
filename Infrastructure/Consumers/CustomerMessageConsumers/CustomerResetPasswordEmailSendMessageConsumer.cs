using Domain.IRepositories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.CustomerEvents;
using FluentEmail.Core;
using Infrastructure.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.CustomerMessageConsumers;

/// <summary>
/// Consumer gửi mail xác nhận reset mật khẩu cho khách hàng
/// </summary>
internal sealed class CustomerResetPasswordEmailSendMessageConsumer : IConsumer<CustomerResetPasswordEmailSendEvent>
{
    private readonly ILogger<CustomerResetPasswordEmailSendMessageConsumer> _logger;
    private readonly IFluentEmail _fluentEmail;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IUnitOfWork _unitOfWork;
    private readonly EmailVerificationLinkFactory _emailVerificationLinkFactory;
    public CustomerResetPasswordEmailSendMessageConsumer(
        IOutBoxMessageServices outBoxMessageServices,
        IUnitOfWork unitOfWork,
        IFluentEmail fluentEmail,
        EmailVerificationLinkFactory emailVerificationLinkFactory,
        ILogger<CustomerResetPasswordEmailSendMessageConsumer> logger)
    {
        _outBoxMessageServices = outBoxMessageServices;
        _unitOfWork = unitOfWork;
        _fluentEmail = fluentEmail;
        _emailVerificationLinkFactory = emailVerificationLinkFactory;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CustomerResetPasswordEmailSendEvent> context)
    {
        var message = context.Message;
        
        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming CustomerResetPasswordEmailSendEvent for email: {email}",
            message.Email);
        //--------------------------------------------------------------
        
        try
        {
            var email = _fluentEmail
                .To(message.Email)
                .Subject("Xác nhận thay đổi mật khẩu")
                .Body($"<h1>Xin chào</h1><p>Để thay đổi mật khẩu, vui lòng nhấn vào <a href='{_emailVerificationLinkFactory.CreateLinkForResetPassword(message.VerificationTokenId)}'>đường dẫn này</a></p>", isHtml: true);
            
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
                "Failed to consume CustomerResetPasswordEmailSendEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume CustomerResetPasswordEmailSendEvent for email: {email}",
                message.Email);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed CustomerResetPasswordEmailSendEvent for email: {email}",
            message.Email);
        //-------------------------------------------------
    }
}
