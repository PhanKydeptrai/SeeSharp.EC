using Domain.IRepositories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.CustomerEvents;
using FluentEmail.Core;
using Infrastructure.Services;
using Microsoft.Extensions.Logging;
using MassTransit;
using SharedKernel;

namespace Infrastructure.Consumers.CustomerMessageConsumer;

internal sealed class CustomerChangePasswordMessageConsumer : IConsumer<CustomerChangePasswordEvent>
{
    private readonly ILogger<CustomerChangePasswordMessageConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFluentEmail _fluentEmail;
    private readonly EmailVerificationLinkFactory _emailVerificationLinkFactory;
    private readonly IOutBoxMessageServices _outBoxMessageServices;

    public CustomerChangePasswordMessageConsumer(
        IUnitOfWork unitOfWork,
        IOutBoxMessageServices outBoxMessageServices,
        ILogger<CustomerChangePasswordMessageConsumer> logger,
        IFluentEmail fluentEmail,
        EmailVerificationLinkFactory emailVerificationLinkFactory)
    {
        _unitOfWork = unitOfWork;
        _outBoxMessageServices = outBoxMessageServices;
        _logger = logger;
        _fluentEmail = fluentEmail;
        _emailVerificationLinkFactory = emailVerificationLinkFactory;
    }

    public async Task Consume(ConsumeContext<CustomerChangePasswordEvent> context)
    {
        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming CustomerChangePasswordEvent for userId: {UserId}",
            context.Message.UserId);
        //--------------------------------------------------------------

        try
        {
            //Consume message
            string verificationLink = _emailVerificationLinkFactory
                .CreateLinkForChangePassword(context.Message.TokenId);

            await _fluentEmail
                .To(context.Message.Email)
                .Subject("Xác nhận thay đổi mật khẩu")
                .Body($"<a href={verificationLink}>Click me</a>", isHtml: true)
                .SendAsync();
            //---------------------------------------------------------------
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                context.Message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume CustomerChangePasswordEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveToMySQL();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume CustomerChangePasswordEvent for userId: {UserId}",
                context.Message.UserId);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Cập nhật trạng thái hoàn thành outbox message -> Commit --
        await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
            context.Message.MessageId,
            OutboxMessageStatus.Processed,
            "Successfully consumed CustomerChangePasswordEvent",
            DateTime.UtcNow);

        await _unitOfWork.SaveToMySQL();
        //----------------------------------------------------------

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed CustomerChangePasswordEvent for userId: {UserId}",
            context.Message.UserId);
        //-------------------------------------------------
    }
}
