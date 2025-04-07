using Domain.IRepositories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.EmployeeEvents;
using FluentEmail.Core;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.EmployeeEventConsumers;

/// <summary>
/// Consumer gửi mail thông báo mật khẩu mới cho nhân viên
/// </summary>
internal sealed class EmployeeResetPasswordEventConsumer : IConsumer<EmployeeResetPasswordEvent>
{
    private readonly ILogger<EmployeeResetPasswordEventConsumer> _logger;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IFluentEmail _fluentEmail;
    private readonly IUnitOfWork _unitOfWork;

    public EmployeeResetPasswordEventConsumer(
        ILogger<EmployeeResetPasswordEventConsumer> logger,
        IOutBoxMessageServices outBoxMessageServices,
        IUnitOfWork unitOfWork,
        IFluentEmail fluentEmail)
    {
        _logger = logger;
        _outBoxMessageServices = outBoxMessageServices;
        _unitOfWork = unitOfWork;
        _fluentEmail = fluentEmail;
    }

    public async Task Consume(ConsumeContext<EmployeeResetPasswordEvent> context)
    {
        var message = context.Message;

        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming EmployeeResetPasswordEvent for email: {email}",
            message.Email);
        //--------------------------------------------------------------

        try
        {
            //Consume message            
            var email = _fluentEmail
                .To(message.Email)
                .Subject("Mật khẩu mới của bạn")
                .Body($"<h1>Xin chào</h1><p>Mật khẩu mới của bạn là: {message.RandomPassword}</p>", isHtml: true);

            await email.SendAsync();

            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                context.Message.MessageId,
                OutboxMessageStatus.Processed,
                string.Empty,
                DateTime.UtcNow);

            await _unitOfWork.SaveChangeAsync();

            //---------------------------------------------------------------
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                context.Message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume EmployeeResetPasswordEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangeAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume EmployeeResetPasswordEvent for email: {email}",
                message.Email);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed EmployeeResetPasswordEvent for email: {email}",
            message.Email);
        //-------------------------------------------------
    }
} 