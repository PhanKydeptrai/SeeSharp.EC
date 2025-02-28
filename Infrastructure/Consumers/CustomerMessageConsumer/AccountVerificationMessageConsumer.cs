using Application.Abstractions.LinkService;
using Domain.IRepositories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.CustomerEvents;
using FluentEmail.Core;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.CustomerMessageConsumer;

internal sealed class AccountVerificationMessageConsumer : IConsumer<AccountVerificationEmailSentEvent>
{
    private readonly ILogger<AccountVerificationEmailSentEvent> _logger;
    private readonly IFluentEmail _fluentEmail;
    private readonly ILinkServices _linkServices;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    public AccountVerificationMessageConsumer(
        ILogger<AccountVerificationEmailSentEvent> logger,
        IFluentEmail fluentEmail,
        IUnitOfWork unitOfWork,
        IOutBoxMessageServices outBoxMessageServices,
        ILinkServices linkServices)
    {
        _logger = logger;
        _fluentEmail = fluentEmail;
        _unitOfWork = unitOfWork;
        _outBoxMessageServices = outBoxMessageServices;
        _linkServices = linkServices;
    }

    public async Task Consume(ConsumeContext<AccountVerificationEmailSentEvent> context)
    {
        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming AccountVerificationEmailSentEvent for userId: {UserId}",
            context.Message.UserId);
        //--------------------------------------------------------------

        try
        {
            // //Consume message
            // Link link = _linkServices.Generate(
            //     EndpointName.Customer.Verify,
            //     new { token = context.Message.VerificationTokenId},
            //     "Verify your account",
            //     EndpointMethod.GET);
                        
            await _fluentEmail
               .To(context.Message.Email)
               .Subject("Welcome to our system")
               .Body($"<a>{context.Message.VerificationTokenId}</a>", true)
               .SendAsync();
            //----------------------------------
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                context.Message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume AccountVerificationEmailSentEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveToMySQL();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume AccountVerificationEmailSentEvent for userId: {UserId}",
                context.Message.UserId);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Cập nhật trạng thái hoàn thành outbox message -> Commit --
        await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
            context.Message.MessageId,
            OutboxMessageStatus.Processed,
            "Successfully consumed AccountVerificationEmailSentEvent",
            DateTime.UtcNow);

        await _unitOfWork.SaveToMySQL();
        //----------------------------------------------------------

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed AccountVerificationEmailSentEvent for userId: {UserId}",
            context.Message.UserId);
        //-------------------------------------------------
    }
}
