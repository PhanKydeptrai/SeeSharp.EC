using Domain.IRepositories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.CustomerEvents;
using FluentEmail.Core;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.CustomerMessageConsumers;
/// <summary>
/// Consumer gửi mail thông báo mật khẩu mới cho khách hàng
/// </summary>
internal sealed class CustomerResetPasswordMessageConsumer : IConsumer<CustomerResetPasswordEvent>
{
    private readonly ILogger<CustomerResetPasswordMessageConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFluentEmail _fluentEmail;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    public CustomerResetPasswordMessageConsumer(
        IUnitOfWork unitOfWork,
        IFluentEmail fluentEmail,
        IOutBoxMessageServices outBoxMessageServices,
        ILogger<CustomerResetPasswordMessageConsumer> logger)
    {
        _unitOfWork = unitOfWork;
        _fluentEmail = fluentEmail;
        _outBoxMessageServices = outBoxMessageServices;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CustomerResetPasswordEvent> context)
    {
        var message = context.Message;
        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming CustomerResetPasswordEvent for userId: {userId}",
            message.UserId);
        //--------------------------------------------------------------
        try
        {
            //Consume message            
            var email = _fluentEmail
            .To(message.Email)
            .Subject("Thông báo thay đổi mật khẩu")
            .Body($"<h1>Xin chào</h1><p>Mật khẩu mới của bạn là: <b>{message.RandomPassword}</b></p>", isHtml: true);

            await email.SendAsync();

            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                message.MessageId,
                OutboxMessageStatus.Processed,
                string.Empty,
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
            //---------------------------------------------------------------
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                context.Message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume CustomerResetPasswordEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume CustomerResetPasswordEvent for userId: {userId}",
                message.UserId);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed CustomerResetPasswordEvent for userId: {userId}",
            message.UserId);
        //-------------------------------------------------

    }
}
