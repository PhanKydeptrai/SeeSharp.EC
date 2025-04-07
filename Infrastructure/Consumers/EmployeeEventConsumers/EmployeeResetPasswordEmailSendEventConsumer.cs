using Domain.IRepositories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.EmployeeEvents;
using FluentEmail.Core;
using Infrastructure.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.EmployeeEventConsumers;

/// <summary>
/// Consumer gửi mail cho nhân viên để đặt lại mật khẩu
/// </summary>
internal sealed class EmployeeResetPasswordEmailSendEventConsumer : IConsumer<EmployeeResetPasswordEmailSendEvent>
{
    private readonly IFluentEmail _fluentEmail;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly EmailVerificationLinkFactory _emailVerificationLinkFactory;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EmployeeResetPasswordEmailSendEventConsumer> _logger;

    public EmployeeResetPasswordEmailSendEventConsumer(
        IFluentEmail fluentEmail,
        IOutBoxMessageServices outBoxMessageServices,
        EmailVerificationLinkFactory emailVerificationLinkFactory,
        IUnitOfWork unitOfWork,
        ILogger<EmployeeResetPasswordEmailSendEventConsumer> logger)
    {
        _fluentEmail = fluentEmail;
        _outBoxMessageServices = outBoxMessageServices;
        _emailVerificationLinkFactory = emailVerificationLinkFactory;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<EmployeeResetPasswordEmailSendEvent> context)
    {
        var message = context.Message;
        
        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming EmployeeResetPasswordEmailSendEvent for email: {email}",
            message.Email);
        //--------------------------------------------------------------
        
        try
        {
            var email = _fluentEmail
                .To(message.Email)
                .Subject("Xác nhận reset mật khẩu")
                .Body($"<h1>Xin chào</h1><p>Để thay đổi mật khẩu, vui lòng nhấn vào <a href='{_emailVerificationLinkFactory.CreateLinkForEmployeeResetPassword(message.VerificationTokenId)}'>đường dẫn này</a></p>", isHtml: true);
            
            await email.SendAsync();

            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                message.MessageId,
                OutboxMessageStatus.Processed,
                string.Empty,
                DateTime.UtcNow);

            await _unitOfWork.SaveChangeAsync();
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume EmployeeResetPasswordEmailSendEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangeAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume EmployeeResetPasswordEmailSendEvent for email: {email}",
                message.Email);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed EmployeeResetPasswordEmailSendEvent for email: {email}",
            message.Email);
        //-------------------------------------------------
    }
} 