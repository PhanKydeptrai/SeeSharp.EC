using Domain.IRepositories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.EmployeeEvents;
using FluentEmail.Core;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.EmployeeEventConsumers;

internal sealed class SendDefaultPasswordToUserEventConsumer : IConsumer<SendDefaultPasswordToUserEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutBoxMessageServices _outboxMessageServices;
    private readonly IFluentEmail _fluentEmail;
    private readonly ILogger<SendDefaultPasswordToUserEventConsumer> _logger;
    
    public SendDefaultPasswordToUserEventConsumer(
        IUnitOfWork unitOfWork, 
        IOutBoxMessageServices outboxMessageServices, 
        IFluentEmail fluentEmail,
        ILogger<SendDefaultPasswordToUserEventConsumer> logger)
    {
        _unitOfWork = unitOfWork;
        _outboxMessageServices = outboxMessageServices;
        _fluentEmail = fluentEmail;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SendDefaultPasswordToUserEvent> context)
    {
        var message = context.Message;
        var email = message.email;
        var randomPassword = message.randomPassword;
        var messageId = message.MessageId;

        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming SendDefaultPasswordToUserEvent for email: {email}",
            email);
        //--------------------------------------------------------------

        try
        {
            // Send the email using FluentEmail
            await _fluentEmail
                .To(email)
                .Subject("Your Default Password")
                .Body($"Your default password is: {randomPassword}")
                .SendAsync();
            
            // Update outbox message status
            await _outboxMessageServices.UpdateOutStatusBoxMessageAsync(
                messageId,
                OutboxMessageStatus.Processed,
                string.Empty,
                DateTime.UtcNow);

            await _unitOfWork.SaveChangeAsync();
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outboxMessageServices.UpdateOutStatusBoxMessageAsync(
                messageId,
                OutboxMessageStatus.Failed,
                "Failed to consume SendDefaultPasswordToUserEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangeAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume SendDefaultPasswordToUserEvent for email: {email}",
                email);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed SendDefaultPasswordToUserEvent for email: {email}",
            email);
        //-------------------------------------------------
    }
}
