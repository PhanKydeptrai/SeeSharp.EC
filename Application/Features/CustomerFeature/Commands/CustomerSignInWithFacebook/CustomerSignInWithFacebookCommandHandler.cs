using Application.Abstractions.Messaging;
using Application.DTOs.Customer;
using Application.IServices;
using Application.Security;
using Domain.IRepositories.Customers;
using Domain.IRepositories.UserAuthenticationTokens;
using Domain.IRepositories.Users;
using Domain.IRepositories;
using SharedKernel;
using Domain.Entities.Users;
using Domain.Entities.Customers;
using Domain.Entities.UserAuthenticationTokens;

namespace Application.Features.CustomerFeature.Commands.CustomerSignInWithFacebook;

internal sealed class CustomerSignInWithFacebookCommandHandler
    : ICommandHandler<CustomerSignInWithFacebookCommand, CustomerSignInResponse>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerQueryServices _customerQueryServices;
    private readonly IUserAuthenticationTokenRepository _userAuthenticationTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenProvider _tokenProvider;
    public CustomerSignInWithFacebookCommandHandler(
        ICustomerRepository customerRepository,
        ICustomerQueryServices customerQueryServices,
        IUserAuthenticationTokenRepository userAuthenticationTokenRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ITokenProvider tokenProvider)
    {
        _customerRepository = customerRepository;
        _customerQueryServices = customerQueryServices;
        _userAuthenticationTokenRepository = userAuthenticationTokenRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _tokenProvider = tokenProvider;
    }

    public async Task<Result<CustomerSignInResponse>> Handle(CustomerSignInWithFacebookCommand request, CancellationToken cancellationToken)
    {
        var customerAuthResponse = await _customerQueryServices.GetCustomerByEmail(Email.FromString(request.email));

        // nếu khách hàng đã tồn tại trong hệ thống -> đăng nhập
        if (customerAuthResponse is not null)
        {
            if (customerAuthResponse.UserStatus == UserStatus.InActive.ToString()) // nếu tài khoản này chưa xác thực
            {
                var customer = await _userRepository.GetUserFromPostgreSQL(
                    UserId.FromUlid(customerAuthResponse.UserId));

                customer!.VerifyAccount();
            }

            var (userAuthenticationToken, response) = CreateAuthenticationToken(customerAuthResponse);

            await _userAuthenticationTokenRepository.AddRefreshToken(userAuthenticationToken);

            await _unitOfWork.SaveChangesAsync();

            return Result.Success(response);
        }

        // nếu khách hàng chưa tồn tại trong hệ thống -> đăng ký
        var user = User.NewUser(
            null,
            UserName.NewUserName(request.userName),
            Email.FromString(request.email),
            PhoneNumber.Empty,
            PasswordHash.Empty,
            null,
            request.imageUrl ?? string.Empty);

        user.VerifyAccount();

        var newCustomer = Customer.NewCustomer(user.UserId, CustomerType.Subscribed);

        //Add customer to database
        await _userRepository.AddUser(user);
        await _customerRepository.AddCustomer(newCustomer);

        string newJti = Ulid.NewUlid().ToGuid().ToString();

        var newAccessToken = _tokenProvider.GenerateAccessTokenForCustomer(
            user.UserId,
            newCustomer.CustomerId,
            user.Email!,
            CustomerType.Subscribed.ToString(),
            newJti);

        var newRefreshToken = _tokenProvider.GenerateRefreshToken();

        // Save jti and refresh token to database
        var newUserAuthenticationToken = UserAuthenticationToken.NewUserAuthenticationToken(
            newRefreshToken,
            newJti,
            DateTime.UtcNow.AddDays(30),
            user.UserId);

        await _userAuthenticationTokenRepository.AddRefreshToken(newUserAuthenticationToken);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success(new CustomerSignInResponse(newAccessToken, newRefreshToken));
    }

    private (UserAuthenticationToken userAuthenticationToken, CustomerSignInResponse customerSignInResponse) CreateAuthenticationToken(
        CustomerAuthenticationResponse customerAuthResponse)
    {
        string jti = Ulid.NewUlid().ToGuid().ToString();

        var accessToken = _tokenProvider.GenerateAccessTokenForCustomer(
            UserId.FromUlid(customerAuthResponse.UserId),
            CustomerId.FromUlid(customerAuthResponse.CustomerId),
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