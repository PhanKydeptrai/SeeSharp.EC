using Domain.IRepositories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.CustomerEvents;
using FluentEmail.Core;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.CustomerMessageConsumer;

internal sealed class CustomerResetPasswordSuccessNotificationMessageConsumer
    : IConsumer<CustomerResetPasswordSuccessNotificationEvent>
{
    private readonly ILogger<CustomerResetPasswordSuccessNotificationMessageConsumer> _logger;
    private readonly IFluentEmail _fluentEmail;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    public CustomerResetPasswordSuccessNotificationMessageConsumer(
        IFluentEmail fluentEmail,
        ILogger<CustomerResetPasswordSuccessNotificationMessageConsumer> logger,
        IUnitOfWork unitOfWork,
        IOutBoxMessageServices outBoxMessageServices)
    {
        _fluentEmail = fluentEmail;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _outBoxMessageServices = outBoxMessageServices;
    }

    public async Task Consume(ConsumeContext<CustomerResetPasswordSuccessNotificationEvent> context)
    {
        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming CustomerResetPasswordSuccessNotificationEvent for UserId: {UserId}",
            context.Message.UserId);
        //--------------------------------------------------------------

        try
        {
            //Consume message
            await _fluentEmail
                .To(context.Message.Email)
                .Subject("Reset mật khẩu thành công")
                .Body($"Mật khẩu mới: {context.Message.RandomPass}")
                .SendAsync();
            //---------------------------------------------------------------
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                context.Message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume CustomerResetPasswordSuccessNotificationEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangeAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume CustomerResetPasswordSuccessNotificationEvent for UserId: {UserId}",
                context.Message.UserId);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Cập nhật trạng thái hoàn thành outbox message -> Commit --
        await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
            context.Message.MessageId,
            OutboxMessageStatus.Processed,
            "Successfully consumed CustomerResetPasswordSuccessNotificationEvent",
            DateTime.UtcNow);

        await _unitOfWork.SaveChangeAsync();
        //----------------------------------------------------------

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed CustomerResetPasswordSuccessNotificationEvent for UserId: {UserId}",
            context.Message.UserId);
        //-------------------------------------------------
    }
}
