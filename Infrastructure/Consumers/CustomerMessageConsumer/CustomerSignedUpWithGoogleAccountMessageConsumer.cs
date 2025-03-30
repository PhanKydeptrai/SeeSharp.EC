using Application.Abstractions.EventBus;
using Domain.Entities.Customers;
using Domain.Entities.Users;
using Domain.IRepositories;
using Domain.IRepositories.Customers;
using Domain.IRepositories.Users;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.CustomerEvents;
using FluentEmail.Core;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.CustomerMessageConsumer;

internal sealed class CustomerSignedUpWithGoogleAccountMessageConsumer
    : IConsumer<CustomerSignedUpWithGoogleAccountEvent>
{
    private readonly ILogger<CustomerSignedUpWithGoogleAccountMessageConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IEventBus _eventBus;
    private readonly IUserRepository _userRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IFluentEmail _fluentEmail;
    public CustomerSignedUpWithGoogleAccountMessageConsumer(
        ILogger<CustomerSignedUpWithGoogleAccountMessageConsumer> logger,
        IOutBoxMessageServices outBoxMessageServices,
        IUnitOfWork unitOfWork,
        IEventBus eventBus,
        IUserRepository userRepository,
        ICustomerRepository customerRepository,
        IFluentEmail fluentEmail)
    {
        _logger = logger;
        _outBoxMessageServices = outBoxMessageServices;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
        _userRepository = userRepository;
        _customerRepository = customerRepository;
        _fluentEmail = fluentEmail;
    }

    public async Task Consume(ConsumeContext<CustomerSignedUpWithGoogleAccountEvent> context)
    {
        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming CustomerSignedUpWithGoogleAccountEvent for customerId: {CustomerId}",
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
                "Failed to consume CustomerSignedUpWithGoogleAccountEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangeAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume CustomerSignedUpWithGoogleAccountEvent for customerId: {CustomerId}",
                context.Message.CustomerId);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Cập nhật trạng thái hoàn thành outbox message -> Commit --

        await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
            context.Message.MessageId,
            OutboxMessageStatus.Processed,
            "Successfully consumed CustomerSignedUpWithGoogleAccountEvent",
            DateTime.UtcNow);

        await _unitOfWork.SaveChangeAsync();
        //----------------------------------------------------------

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed CustomerSignedUpWithGoogleAccountEvent for customerId: {CustomerId}",
            context.Message.CustomerId);
        //-------------------------------------------------
    }

    #region Private methods
    private User CreateNewUserFromEvent(CustomerSignedUpWithGoogleAccountEvent request)
    {
        //FromExisting
        return User.NewUser(
            UserId.FromGuid(request.UserId),
            UserName.FromString(request.UserName),
            Domain.Entities.Users.Email.FromString(request.Email),
            PhoneNumber.Empty,
            PasswordHash.Empty,
            null,
            request.ImageUrl);
    }

    private Customer CreateCustomerFromEvent(User user, CustomerSignedUpWithGoogleAccountEvent context)
    {
        return Customer.FromExisting(
                CustomerId.FromGuid(context.CustomerId),
                user.UserId,
                CustomerStatus.Active,
                CustomerType.Subscribed);
    }
    #endregion
}
