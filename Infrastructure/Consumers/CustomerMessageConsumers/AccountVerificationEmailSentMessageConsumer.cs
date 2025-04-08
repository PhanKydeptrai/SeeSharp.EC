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
/// Consumer gửi mail xác nhận tài khoản cho khách hàng (Để kích hoạt tài khoản)
/// </summary>
internal sealed class AccountVerificationEmailSentMessageConsumer : IConsumer<AccountVerificationEmailSentEvent>
{
    private readonly IFluentEmail _fluentEmail;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly EmailVerificationLinkFactory _emailVerificationLinkFactory;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AccountVerificationEmailSentMessageConsumer> _logger;

    public AccountVerificationEmailSentMessageConsumer(
        IFluentEmail fluentEmail,
        IOutBoxMessageServices outBoxMessageServices,
        EmailVerificationLinkFactory emailVerificationLinkFactory,
        IUnitOfWork unitOfWork,
        ILogger<AccountVerificationEmailSentMessageConsumer> logger)
    {
        _fluentEmail = fluentEmail;
        _outBoxMessageServices = outBoxMessageServices;
        _emailVerificationLinkFactory = emailVerificationLinkFactory;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AccountVerificationEmailSentEvent> context)
    {
        var message = context.Message;
        
        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming AccountVerificationEmailSentEvent for email: {email}",
            message.Email);
        //--------------------------------------------------------------
        
        try
        {
            var email = _fluentEmail
                .To(message.Email)
                .Subject("Xác nhận tài khoản")
                .Body($"<h1>Xin chào</h1><p>Để xác nhận tài khoản, vui lòng nhấn vào <a href='{_emailVerificationLinkFactory.CreateLinkForEmailVerification(message.VerificationTokenId)}'>đường dẫn này</a></p>", isHtml: true);
            
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
                "Failed to consume AccountVerificationEmailSentEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume AccountVerificationEmailSentEvent for email: {email}",
                message.Email);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed AccountVerificationEmailSentEvent for email: {email}",
            message.Email);
        //-------------------------------------------------
    }
}
