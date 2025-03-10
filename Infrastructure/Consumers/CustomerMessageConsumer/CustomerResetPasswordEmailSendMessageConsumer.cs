using Domain.IRepositories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.CustomerEvents;
using FluentEmail.Core;
using Infrastructure.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.CustomerMessageConsumer;

internal sealed class CustomerResetPasswordEmailSendMessageConsumer : IConsumer<CustomerResetPasswordEmailSendEvent>
{
    private readonly ILogger<CustomerResetPasswordEmailSendMessageConsumer> _logger;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IUnitOfWork _unitOfWork;
    private readonly EmailVerificationLinkFactory _emailVerificationLinkFactory;
    private readonly IFluentEmail _fluentEmail;
    public CustomerResetPasswordEmailSendMessageConsumer(
        ILogger<CustomerResetPasswordEmailSendMessageConsumer> logger,
        IOutBoxMessageServices outBoxMessageServices,
        IUnitOfWork unitOfWork,
        IFluentEmail fluentEmail,
        EmailVerificationLinkFactory emailVerificationLinkFactory)
    {
        _logger = logger;
        _outBoxMessageServices = outBoxMessageServices;
        _unitOfWork = unitOfWork;
        _fluentEmail = fluentEmail;
        _emailVerificationLinkFactory = emailVerificationLinkFactory;
    }

    public async Task Consume(ConsumeContext<CustomerResetPasswordEmailSendEvent> context)
    {
        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming CustomerResetPasswordEmailSendEvent for UserId: {UserId}",
            context.Message.UserId);
        //--------------------------------------------------------------

        try
        {
            //Consume message            
            string verificationLink = _emailVerificationLinkFactory
                .CreateLinkForResetPassword(context.Message.VerificationTokenId);

            await _fluentEmail
                .To(context.Message.Email)
                .Subject("Xác nhận đặt lại mật khẩu")
                .Body($"<a href='{verificationLink}'>Click me</a>", isHtml: true)
                .SendAsync();
            
            //---------------------------------------------------------------
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                context.Message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume CustomerResetPasswordEmailSendEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveToMySQL();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume CustomerResetPasswordEmailSendEvent for UserId: {UserId}",
                context.Message.UserId);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Cập nhật trạng thái hoàn thành outbox message -> Commit --
        await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
            context.Message.MessageId,
            OutboxMessageStatus.Processed,
            "Successfully consumed CustomerResetPasswordEmailSendEvent",
            DateTime.UtcNow);

        await _unitOfWork.SaveToMySQL();
        //----------------------------------------------------------

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed CustomerResetPasswordEmailSendEvent for UserId: {UserId}",
            context.Message.UserId);
        //-------------------------------------------------
    }
}
