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
/// Consumer gửi mail xác nhận thay đổi mật khẩu cho nhân viên
/// </summary>
internal sealed class EmployeeChangePasswordEventConsumer : IConsumer<EmployeeChangePasswordEvent>
{
    private readonly ILogger<EmployeeChangePasswordEventConsumer> _logger;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IFluentEmail _fluentEmail;
    private readonly IUnitOfWork _unitOfWork;
    private readonly EmailVerificationLinkFactory _emailVerificationLinkFactory;

    public EmployeeChangePasswordEventConsumer(
        IOutBoxMessageServices outBoxMessageServices,
        IUnitOfWork unitOfWork,
        ILogger<EmployeeChangePasswordEventConsumer> logger,
        IFluentEmail fluentEmail,
        EmailVerificationLinkFactory emailVerificationLinkFactory)
    {
        _outBoxMessageServices = outBoxMessageServices;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _fluentEmail = fluentEmail;
        _emailVerificationLinkFactory = emailVerificationLinkFactory;
    }

    public async Task Consume(ConsumeContext<EmployeeChangePasswordEvent> context)
    {
        var message = context.Message;
        
        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming EmployeeChangePasswordEvent for email: {email}",
            message.Email);
        //--------------------------------------------------------------
        
        try
        {
            var email = _fluentEmail
                .To(message.Email)
                .Subject("Xác nhận thay đổi mật khẩu")
                .Body($"<h1>Xin chào</h1><p>Để thay đổi mật khẩu, vui lòng nhấn vào <a href='{_emailVerificationLinkFactory.CreateLinkForEmployeeChangePassword(message.VerificationTokenId)}'>đường dẫn này</a></p>", isHtml: true);

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
                "Failed to consume EmployeeChangePasswordEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume EmployeeChangePasswordEvent for email: {email}",
                message.Email);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed EmployeeChangePasswordEvent for email: {email}",
            message.Email);
        //-------------------------------------------------
    }
} 