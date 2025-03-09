using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Application.DTOs.Customer;
using Application.IServices;
using Application.Outbox;
using Application.Security;
using Domain.Entities.Customers;
using Domain.Entities.UserAuthenticationTokens;
using Domain.Entities.Users;
using Domain.IRepositories;
using Domain.IRepositories.Customers;
using Domain.IRepositories.UserAuthenticationTokens;
using Domain.IRepositories.Users;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Errors;
using Domain.Utilities.Events.CustomerEvents;
using Google.Apis.Auth;
using SharedKernel;

namespace Application.Features.CustomerFeature.Commands.CustomerSignInWithGoogle;

internal sealed class CustomerSignInWithGoogleCommandHandler
    : ICommandHandler<CustomerSignInWithGoogleCommand, CustomerSignInResponse>
{
    #region Dependencies
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerQueryServices _customerQueryServices;
    private readonly IUserAuthenticationTokenRepository _userAuthenticationTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenProvider _tokenProvider;
    private readonly IEventBus _eventBus;
    private readonly IOutBoxMessageServices _outboxMessageServices;
    public CustomerSignInWithGoogleCommandHandler(
        ICustomerRepository customerRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IOutBoxMessageServices outboxMessageServices,
        IEventBus eventBus,
        ICustomerQueryServices customerQueryServices,
        ITokenProvider tokenProvider,
        IUserAuthenticationTokenRepository userAuthenticationTokenRepository)
    {
        _customerRepository = customerRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _outboxMessageServices = outboxMessageServices;
        _eventBus = eventBus;
        _customerQueryServices = customerQueryServices;
        _tokenProvider = tokenProvider;
        _userAuthenticationTokenRepository = userAuthenticationTokenRepository;
    }
    #endregion

    public async Task<Result<CustomerSignInResponse>> Handle(
        CustomerSignInWithGoogleCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var googleUser = await GoogleJsonWebSignature.ValidateAsync(
                request.token, new GoogleJsonWebSignature.ValidationSettings());

            var customerAuthResponse = await _customerQueryServices.GetCustomerByEmail(Email.FromString(googleUser.Email));

            // If customer exist -> sign in
            if (customerAuthResponse is not null)
            {
                if (customerAuthResponse.UserStatus == UserStatus.InActive.ToString()) // if this account not verify
                {
                    var customer = await _userRepository.GetUserFromMySQL(
                        UserId.FromUlid(customerAuthResponse.UserId));

                    customer!.VerifyAccount();
                }
                
                var (userAuthenticationToken, response) = CreateAuthenticationToken(customerAuthResponse);

                await _userAuthenticationTokenRepository.AddRefreshTokenToMySQL(userAuthenticationToken);
                
                await _unitOfWork.SaveToMySQL();

                return Result.Success(response);
            }

            // If customer not exist -> sign up
            var user = User.NewUser(
                null,
                UserName.NewUserName(googleUser.Name),
                Email.FromString(googleUser.Email),
                PhoneNumber.Empty,
                PasswordHash.Empty,
                null,
                googleUser.Picture);

            user.VerifyAccount();

            var newCustomer = Customer.NewCustomer(user.UserId, CustomerType.Subscribed);
            
            //Add customer to database
            await _userRepository.AddUserToMySQL(user);
            await _customerRepository.AddCustomerToMySQL(newCustomer);

            string newJti = Ulid.NewUlid().ToGuid().ToString();

            var newAccessToken = _tokenProvider.GenerateAccessToken(
                user.UserId,
                user.Email!,
                CustomerType.Subscribed.ToString(),
                newJti);

            var newRefreshToken = _tokenProvider.GenerateRefreshToken();

            // Save jti and refresh token to database
            var newUserAuthenticationToken = UserAuthenticationToken.NewUserAuthenticationToken(
                newAccessToken,
                newJti,
                DateTime.UtcNow.AddDays(30),
                user.UserId);

            await _userAuthenticationTokenRepository.AddRefreshTokenToMySQL(newUserAuthenticationToken);

            var message = new CustomerSignedUpWithGoogleAccountEvent(
                user.UserId, 
                newCustomer.CustomerId, 
                user.UserName.Value,
                user.Email!.Value,
                user.ImageUrl! ?? string.Empty,
                Ulid.NewUlid().ToGuid());

            await OutboxMessageExtentions.InsertOutboxMessageAsync(
                message.MessageId ,message, _outboxMessageServices);
            
            await _unitOfWork.SaveToMySQL();

            await _eventBus.PublishAsync(message);

            return Result.Success(new CustomerSignInResponse(newAccessToken, newRefreshToken));
        }
        catch
        {
            return Result.Failure<CustomerSignInResponse>(CustomerError.InValidToken());
        }

    }


    private (UserAuthenticationToken userAuthenticationToken, CustomerSignInResponse customerSignInResponse) CreateAuthenticationToken(
        CustomerAuthenticationResponse customerAuthResponse)
    {
        string jti = Ulid.NewUlid().ToGuid().ToString();

        var accessToken = _tokenProvider.GenerateAccessToken(
            UserId.FromUlid(customerAuthResponse.UserId),
            Email.FromString(customerAuthResponse.Email),
            customerAuthResponse.CustomerType.ToString(), jti);

        var refreshToken = _tokenProvider.GenerateRefreshToken();

        // Save jti and refresh token to database
        var userAuthenticationToken = UserAuthenticationToken.NewUserAuthenticationToken(
            refreshToken,
            jti,
            DateTime.UtcNow.AddDays(30),
            UserId.FromUlid(customerAuthResponse.UserId));


        return (userAuthenticationToken, new CustomerSignInResponse(accessToken, refreshToken));

    }
}