using Domain.IRepositories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.CustomerEvents;
using FluentEmail.Core;
using Infrastructure.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.CustomerMessageConsumers;

internal sealed class CustomerChangePasswordEventConsumer : IConsumer<CustomerChangePasswordEvent>
{
    private readonly ILogger<CustomerChangePasswordEvent> _logger;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IFluentEmail _fluentEmail;
    private readonly IUnitOfWork _unitOfWork;
    private readonly EmailVerificationLinkFactory _emailVerificationLinkFactory;

    public CustomerChangePasswordEventConsumer(
        IOutBoxMessageServices outBoxMessageServices,
        IUnitOfWork unitOfWork,
        ILogger<CustomerChangePasswordEvent> logger,
        IFluentEmail fluentEmail,
        EmailVerificationLinkFactory emailVerificationLinkFactory)
    {
        _outBoxMessageServices = outBoxMessageServices;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _fluentEmail = fluentEmail;
        _emailVerificationLinkFactory = emailVerificationLinkFactory;
    }

    public async Task Consume(ConsumeContext<CustomerChangePasswordEvent> context)
    {
        var message = context.Message;
        
        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming CustomerChangePasswordEvent for email: {email}",
            message.Email);
        //--------------------------------------------------------------
        
        try
        {
            var email = _fluentEmail
            .To(message.Email)
            .Subject("Xác nhận thay đổi mật khẩu")
            .Body($"<h1>Xin chào</h1><p>Để thay đổi mật khẩu, vui lòng nhấn vào <a href='{_emailVerificationLinkFactory.CreateLinkForChangePassword(message.VerificationTokenId)}'>đường dẫn này</a></p>", isHtml: true);

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
                "Failed to consume CustomerChangePasswordEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangeAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume CustomerChangePasswordEvent for email: {email}",
                message.Email);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed CustomerChangePasswordEvent for email: {email}",
            message.Email);
        //-------------------------------------------------
    }
}
