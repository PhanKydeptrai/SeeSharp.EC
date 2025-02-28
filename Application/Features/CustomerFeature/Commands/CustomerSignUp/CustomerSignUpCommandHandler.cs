using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Application.Outbox;
using Domain.Entities.Customers;
using Domain.Entities.Users;
using Domain.Entities.VerificationTokens;
using Domain.IRepositories;
using Domain.IRepositories.Customers;
using Domain.IRepositories.Users;
using Domain.IRepositories.VerificationTokens;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.CustomerEvents;
using NETCore.Encrypt.Extensions;
using SharedKernel;

namespace Application.Features.CustomerFeature.Commands.CustomerSignUp;

internal sealed class CustomerSignUpCommandHandler : ICommandHandler<CustomerSignUpCommand>
{
    #region Dependencies
    private readonly ICustomerRepository _customerRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IEventBus _eventBus;
    private readonly IVerificationTokenRepository _verificationTokenRepository;

    public CustomerSignUpCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        IEventBus eventBus,
        IUserRepository userRepository,
        IOutBoxMessageServices outBoxMessageServices,
        IVerificationTokenRepository verificationTokenRepository)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
        _userRepository = userRepository;
        _outBoxMessageServices = outBoxMessageServices;
        _verificationTokenRepository = verificationTokenRepository;
    }
    #endregion

    public async Task<Result> Handle(
        CustomerSignUpCommand request,
        CancellationToken cancellationToken)
    {
        var user = CreateNewUser(request);
        var customer = Customer.NewCustomer(user.UserId, CustomerType.Subscribed);
        var verificationToken = VerificationToken.NewVerificationToken(
            string.Empty, user.UserId, DateTime.UtcNow.AddMinutes(5));

        var message = CreateCustomerSignedUpEvent(user, customer, verificationToken);

        await _userRepository.AddUserToMySQL(user);
        await _customerRepository.AddCustomerToMySQL(customer);
        await _verificationTokenRepository.AddVerificationTokenToMySQL(verificationToken);

        await OutboxMessageExtentions.InsertOutboxMessageAsync(
            message.MessageId, 
            message, 
            _outBoxMessageServices);
    
        await _unitOfWork.SaveToMySQL();

        await _eventBus.PublishAsync(message);
        return Result.Success();
    }
    
    private CustomerSignedUpEvent CreateCustomerSignedUpEvent(User user, Customer customer, VerificationToken verificationToken)
    {
        return new CustomerSignedUpEvent(
            user.UserId,
            customer.CustomerId,
            verificationToken.VerificationTokenId,
            user.UserName.Value!,
            user.Email!.Value,
            user.PasswordHash!.Value,
            Ulid.NewUlid().ToGuid());
    }


    private User CreateNewUser(CustomerSignUpCommand request)
    {
        return User.NewUser(
            UserName.NewUserName(request.UserName),
            Email.NewEmail(request.Email),
            PhoneNumber.Empty,
            PasswordHash.NewPasswordHash(request.Password.SHA256()),
            null,
            string.Empty);
    }
}
