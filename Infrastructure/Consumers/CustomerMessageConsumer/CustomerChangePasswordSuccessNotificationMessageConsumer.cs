using Domain.IRepositories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.CustomerEvents;
using FluentEmail.Core;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.CustomerMessageConsumer;

internal sealed class CustomerChangePasswordSuccessNotificationMessageConsumer
    : IConsumer<CustomerChangePasswordSuccessNotificationEvent>
{
    private readonly ILogger<CustomerChangePasswordSuccessNotificationEvent> _logger;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFluentEmail _fluentEmail;
    public CustomerChangePasswordSuccessNotificationMessageConsumer(
        ILogger<CustomerChangePasswordSuccessNotificationEvent> logger,
        IFluentEmail fluentEmail,
        IUnitOfWork unitOfWork,
        IOutBoxMessageServices outBoxMessageServices)
    {
        _logger = logger;
        _fluentEmail = fluentEmail;
        _unitOfWork = unitOfWork;
        _outBoxMessageServices = outBoxMessageServices;
    }

    public async Task Consume(ConsumeContext<CustomerChangePasswordSuccessNotificationEvent> context)
    {
        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming CustomerChangePasswordSuccessNotificationEvent for {email}",
            context.Message.MessageId);
        //--------------------------------------------------------------

        try
        {
            //Consume message            

            await _fluentEmail
                .To(context.Message.email)
                .Subject("Đổi mật khẩu thành công")
                .Body("Mật khẩu của bạn đã được thay đổi thành công!")
                .SendAsync();
            //---------------------------------------------------------------
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                context.Message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume CustomerChangePasswordSuccessNotificationEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangeAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume CustomerChangePasswordSuccessNotificationEvent for {email}",
                context.Message.MessageId);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Cập nhật trạng thái hoàn thành outbox message -> Commit --
        await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
            context.Message.MessageId,
            OutboxMessageStatus.Processed,
            "Successfully consumed CustomerChangePasswordSuccessNotificationEvent",
            DateTime.UtcNow);

        await _unitOfWork.SaveChangeAsync();
        //----------------------------------------------------------

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed CustomerChangePasswordSuccessNotificationEvent for {email}",
            context.Message.MessageId);
        //-------------------------------------------------
    }
}
