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
using Domain.Utilities.Errors;
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


    public async Task<Result> Handle(CustomerSignUpCommand request, CancellationToken cancellationToken)
    {
        var account = await _customerRepository.GetCustomerByEmailFromPostgreSQL(Email.FromString(request.Email));

        if (account is not null && account.User!.IsVerify == IsVerify.True)
        {
            return Result.Failure(CustomerError.EmailAlreadyInUse());
        }
        
        if (account is not null && account.User!.IsVerify == IsVerify.False)
        {
            var newVerificationToken = VerificationToken.NewVerificationToken(
                string.Empty, account.UserId, DateTime.UtcNow.AddMinutes(5));

            await _verificationTokenRepository.AddVerificationTokenToPostgreSQL(newVerificationToken);

            var newMessage = new AccountVerificationEmailSentEvent(
                account.UserId,
                newVerificationToken.VerificationTokenId,
                account.User.Email!.Value,
                Ulid.NewUlid().ToGuid());

            await OutboxMessageExtentions.InsertOutboxMessageAsync(
                newMessage.MessageId,
                newMessage, _outBoxMessageServices);

            await _unitOfWork.SaveChangesAsync();
            await _eventBus.PublishAsync(newMessage);
            return Result.Success();
        }

        var user = CreateNewUser(request);
        var customer = Customer.NewCustomer(user.UserId, CustomerType.Subscribed);
        var verificationToken = VerificationToken.NewVerificationToken(
            string.Empty, user.UserId, DateTime.UtcNow.AddMinutes(5));

        var message = new AccountVerificationEmailSentEvent(
                user.UserId,
                verificationToken.VerificationTokenId,
                user.Email!.Value,
                Ulid.NewUlid().ToGuid());

        await _userRepository.AddUser(user);
        await _customerRepository.AddCustomer(customer);
        await _verificationTokenRepository.AddVerificationTokenToPostgreSQL(verificationToken);

        await OutboxMessageExtentions.InsertOutboxMessageAsync(
            message.MessageId,
            message,
            _outBoxMessageServices);

        await _unitOfWork.SaveChangesAsync();

        await _eventBus.PublishAsync(message);
        
        return Result.Success();
    }

    // Private method
    private User CreateNewUser(CustomerSignUpCommand request)
    {
        return User.NewUser(
            null,
            UserName.NewUserName(request.UserName),
            Email.NewEmail(request.Email),
            PhoneNumber.Empty,
            PasswordHash.NewPasswordHash(request.Password.SHA256()),
            null,
            string.Empty);
    }
}