using Application.Abstractions.EventBus;
using Application.Outbox;
using Domain.Entities.Users;
using Domain.IRepositories;
using Domain.IRepositories.Users;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.CustomerEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.CustomerMessageConsumer;

internal sealed class CustomerConfirmChangePasswordMessageConsumer
    : IConsumer<CustomerConfirmChangePasswordEvent>
{
    private readonly ILogger<CustomerConfirmChangePasswordMessageConsumer> _logger;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;
    public CustomerConfirmChangePasswordMessageConsumer(
        ILogger<CustomerConfirmChangePasswordMessageConsumer> logger,
        IUserRepository userRepository,
        IOutBoxMessageServices outBoxMessageServices,
        IUnitOfWork unitOfWork,
        IEventBus eventBus)
    {
        _logger = logger;
        _userRepository = userRepository;
        _outBoxMessageServices = outBoxMessageServices;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
    }

    public async Task Consume(ConsumeContext<CustomerConfirmChangePasswordEvent> context)
    {
        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming CustomerConfirmChangePasswordEvent for userId: {[id]}",
            context.Message.UserId);
        //--------------------------------------------------------------

        try
        {
            //Consume message            
            var user = await _userRepository.GetUserFromPostgreSQL(UserId.FromGuid(context.Message.UserId));
            user!.ChangePassword(PasswordHash.FromString(context.Message.NewPassword));

            await _unitOfWork.SaveChangeAsync();
            //---------------------------------------------------------------
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                context.Message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume CustomerConfirmChangePasswordEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangeAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume CustomerConfirmChangePasswordEvent for userId: {[id]}",
                context.Message.UserId);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Cập nhật trạng thái hoàn thành outbox message -> Commit --
        await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
            context.Message.MessageId,
            OutboxMessageStatus.Processed,
            "Successfully consumed CustomerConfirmChangePasswordEvent",
            DateTime.UtcNow);

        await _unitOfWork.SaveChangeAsync();
        //----------------------------------------------------------

        //Publish event notify user
        var message = new CustomerChangePasswordSuccessNotificationEvent(
            context.Message.Email, 
            Ulid.NewUlid().ToGuid());

        await OutboxMessageExtentions.InsertOutboxMessageAsync(
            message.MessageId, 
            message, _outBoxMessageServices);

        await _unitOfWork.SaveChangeAsync();
        
        await _eventBus.PublishAsync(message);

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed CustomerConfirmChangePasswordEvent for userId: {[id]}",
            context.Message.UserId);
        //-------------------------------------------------
    }
}
