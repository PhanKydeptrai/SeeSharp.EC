using Application.Outbox;
using Domain.Entities.Customers;
using Domain.Entities.Users;
using Domain.IRepositories;
using Domain.IRepositories.Customers;
using Domain.IRepositories.Users;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.CustomerEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.CustomerMessageConsumer;

internal sealed class CustomerSignedUpMessageConsumer : IConsumer<CustomerSignedUpEvent>
{
    #region Dependencies
    private readonly ILogger<CustomerSignedUpMessageConsumer> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutBoxMessageServices _outBoxMessageServices;

    public CustomerSignedUpMessageConsumer(
        ILogger<CustomerSignedUpMessageConsumer> logger,
        ICustomerRepository customerRepository,
        IOutBoxMessageServices outBoxMessageServices,
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _customerRepository = customerRepository;
        _outBoxMessageServices = outBoxMessageServices;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _publishEndpoint = publishEndpoint;
    }
    #endregion

    public async Task Consume(ConsumeContext<CustomerSignedUpEvent> context)
    {
        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming CustomerSignedUpEvent for customerId: {CustomerId}",
            context.Message.CustomerId);
        //--------------------------------------------------------------

        try
        {
            //Consume message---------------------------------------------          
            var user = CreateNewUserFromEvent(context.Message);
            var customer = CreateCustomerFromEvent(user, context.Message);

            await _userRepository.AddUserToPostgreSQL(user);
            await _customerRepository.AddCustomerToPostgreSQL(customer);
            await _unitOfWork.SaveChangeAsync();
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                context.Message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume CustomerSignedUpEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangeAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume CustomerSignedUpEvent for customerId: {CustomerId}",
                context.Message.CustomerId);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Cập nhật trạng thái hoàn thành outbox message -> Commit --

        await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
            context.Message.MessageId,
            OutboxMessageStatus.Processed,
            "Successfully consumed CustomerSignedUpEvent",
            DateTime.UtcNow);
        //----------------------------------------------------------

        //Publish event-----------------------------------------------
        var message = new AccountVerificationEmailSentEvent(
            context.Message.UserId,
            context.Message.VerificationTokenId,
            context.Message.Email,
            Ulid.NewUlid().ToGuid());

        await OutboxMessageExtentions.InsertOutboxMessageAsync(
            message.MessageId,
            message,
            _outBoxMessageServices);

        await _unitOfWork.SaveChangeAsync();

        await _publishEndpoint.Publish(message);


        //------------------------------------------------------------



        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed CustomerSignedUpEvent for customerId: {CustomerId}",
            context.Message.CustomerId);
        //-------------------------------------------------


    }


    #region Private methods
    private User CreateNewUserFromEvent(CustomerSignedUpEvent request)
    {
        //FromExisting
        return User.NewUser(
            UserId.FromGuid(request.UserId),
            UserName.FromString(request.UserName),
            Domain.Entities.Users.Email.FromString(request.Email),
            PhoneNumber.Empty,
            PasswordHash.FromString(request.PasswordHash),
            null,
            string.Empty);
    }

    private Customer CreateCustomerFromEvent(User user, CustomerSignedUpEvent context)
    {
        return Customer.FromExisting(
                CustomerId.FromGuid(context.CustomerId),
                user.UserId,
                CustomerStatus.Active,
                CustomerType.Subscribed);
    }
    #endregion
}
