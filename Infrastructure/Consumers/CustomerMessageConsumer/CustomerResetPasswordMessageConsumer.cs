using Application.Abstractions.EventBus;
using Application.Outbox;
using Domain.Entities.Users;
using Domain.IRepositories;
using Domain.IRepositories.Users;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.CustomerEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using NETCore.Encrypt.Extensions;
using SharedKernel;

namespace Infrastructure.Consumers.CustomerMessageConsumer;

internal class CustomerResetPasswordMessageConsumer : IConsumer<CustomerResetPasswordEvent>
{
    private readonly ILogger<CustomerResetPasswordMessageConsumer> _logger;
    private readonly IEventBus _eventBus;
    private readonly IUserRepository _userRepository;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IUnitOfWork _unitOfWork;

    public CustomerResetPasswordMessageConsumer(
        ILogger<CustomerResetPasswordMessageConsumer> logger,
        IEventBus eventBus,
        IUserRepository userRepository,
        IOutBoxMessageServices outBoxMessageServices,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _eventBus = eventBus;
        _userRepository = userRepository;
        _outBoxMessageServices = outBoxMessageServices;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<CustomerResetPasswordEvent> context)
    {
        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming CustomerResetPasswordEvent for UserId: {[id]}",
            context.Message.UserId);
        //--------------------------------------------------------------

        try
        {
            //Consume message            
            var user = await _userRepository.GetUserFromPostgreSQL(UserId.FromGuid(context.Message.UserId));
            user!.ChangePassword(PasswordHash.FromString(context.Message.RandomPass.SHA256()));
            await _unitOfWork.SaveChangeAsync();
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

            await _unitOfWork.SaveChangeAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume CustomerResetPasswordEvent for UserId: {[id]}",
                context.Message.UserId);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Cập nhật trạng thái hoàn thành outbox message -> Commit --
        await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
            context.Message.MessageId,
            OutboxMessageStatus.Processed,
            "Successfully consumed CustomerResetPasswordEvent",
            DateTime.UtcNow);
            
        //Create message
        var message = new CustomerResetPasswordSuccessNotificationEvent(
            context.Message.UserId, 
            context.Message.Email,
            context.Message.RandomPass,
            Ulid.NewUlid().ToGuid());

        await OutboxMessageExtentions.InsertOutboxMessageAsync(
            message.MessageId ,message, 
            _outBoxMessageServices);

        await _unitOfWork.SaveChangeAsync();
        await _eventBus.PublishAsync(message);
        //----------------------------------------------------------

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed CustomerResetPasswordEvent for UserId: {[id]}",
            context.Message.UserId);
        //-------------------------------------------------
    }
}
