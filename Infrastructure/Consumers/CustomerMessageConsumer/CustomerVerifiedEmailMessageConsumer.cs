using Domain.Entities.Users;
using Domain.IRepositories;
using Domain.IRepositories.Users;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.CustomerEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.CustomerMessageConsumer;

internal sealed class CustomerVerifiedEmailMessageConsumer : IConsumer<CustomerVerifiedEmailEvent>
{
    #region Depen
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CustomerVerifiedEmailMessageConsumer> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IOutBoxMessageServices _outBoxMessageServices;
        public CustomerVerifiedEmailMessageConsumer(
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IOutBoxMessageServices outBoxMessageServices,
            ILogger<CustomerVerifiedEmailMessageConsumer> logger)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _outBoxMessageServices = outBoxMessageServices;
            _logger = logger;
        }
    
    #endregion
    public async Task Consume(ConsumeContext<CustomerVerifiedEmailEvent> context)
    {
        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming CustomerVerifiedEmailEvent for userId: {UserId}",
            context.Message.UserId);
        //--------------------------------------------------------------

        try
        {
            //Consume message
            var userId = UserId.FromGuid(context.Message.UserId);
            var user = await _userRepository.GetUserFromPostgreSQL(userId);
            user!.VerifyAccount();
            await _unitOfWork.SaveChangeAsync();
            //---------------------------------------------------------------
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                context.Message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume CustomerVerifiedEmailEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangeAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume CustomerVerifiedEmailEvent for userId: {UserId}",
                context.Message.UserId);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Cập nhật trạng thái hoàn thành outbox message -> Commit --
        await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
            context.Message.MessageId,
            OutboxMessageStatus.Processed,
            "Successfully consumed CustomerVerifiedEmailEvent",
            DateTime.UtcNow);

        await _unitOfWork.SaveChangeAsync();
        //----------------------------------------------------------

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed CustomerVerifiedEmailEvent for userId: {UserId}",
            context.Message.UserId);
        //-------------------------------------------------
    }
}
