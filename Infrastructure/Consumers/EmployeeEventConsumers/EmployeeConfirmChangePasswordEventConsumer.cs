using Domain.IRepositories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.EmployeeEvents;
using FluentEmail.Core;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.EmployeeEventConsumers;

/// <summary>
/// Consumer gửi mail thông báo xác nhận thay đổi mật khẩu thành công cho nhân viên
/// </summary>
internal sealed class EmployeeConfirmChangePasswordEventConsumer : IConsumer<EmployeeConfirmChangePasswordEvent>
{
    private readonly ILogger<EmployeeConfirmChangePasswordEventConsumer> _logger;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IFluentEmail _fluentEmail;
    private readonly IUnitOfWork _unitOfWork;

    public EmployeeConfirmChangePasswordEventConsumer(
        ILogger<EmployeeConfirmChangePasswordEventConsumer> logger,
        IOutBoxMessageServices outBoxMessageServices,
        IUnitOfWork unitOfWork,
        IFluentEmail fluentEmail)
    {
        _logger = logger;
        _outBoxMessageServices = outBoxMessageServices;
        _unitOfWork = unitOfWork;
        _fluentEmail = fluentEmail;
    }

    public async Task Consume(ConsumeContext<EmployeeConfirmChangePasswordEvent> context)
    {
        var message = context.Message;

        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming EmployeeConfirmChangePasswordEvent for email: {email}",
            message.Email);
        //--------------------------------------------------------------

        try
        {
            //Consume message            
            var email = _fluentEmail
                .To(message.Email)
                .Subject("Xác nhận thay đổi mật khẩu thành công")
                .Body($"<h1>Xin chào</h1><p>Mật khẩu của bạn đã được thay đổi thành công!</p>", isHtml: true);

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
                message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume EmployeeConfirmChangePasswordEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume EmployeeConfirmChangePasswordEvent for email: {email}",
                message.Email);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed EmployeeConfirmChangePasswordEvent for email: {email}",
            message.Email);
        //-------------------------------------------------
    }
} 