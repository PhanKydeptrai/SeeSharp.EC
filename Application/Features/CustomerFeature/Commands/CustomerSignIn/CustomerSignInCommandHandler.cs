using Application.Abstractions.Messaging;
using Application.DTOs.Customer;
using Application.IServices;
using Application.Security;
using Domain.Entities.Customers;
using Domain.Entities.UserAuthenticationTokens;
using Domain.Entities.Users;
using Domain.IRepositories;
using Domain.IRepositories.Orders;
using Domain.IRepositories.UserAuthenticationTokens;
using Domain.Utilities.Errors;
using NETCore.Encrypt.Extensions;
using SharedKernel;

namespace Application.Features.CustomerFeature.Commands.CustomerSignIn;

internal sealed class CustomerSignInCommandHandler : ICommandHandler<CustomerSignInCommand, CustomerSignInResponse>
{
    #region Dependencies
    private readonly ICustomerQueryServices _customerQueryServices;
    private readonly IUserAuthenticationTokenRepository _userAuthenticationTokenRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUnitOfWork _unitOfWork;
    public CustomerSignInCommandHandler(
        IUnitOfWork unitOfWork,
        ICustomerQueryServices customerQueryServices,
        ITokenProvider tokenProvider,
        IUserAuthenticationTokenRepository userAuthenticationTokenRepository,
        IOrderRepository orderRepository)
    {
        _unitOfWork = unitOfWork;
        _customerQueryServices = customerQueryServices;
        _tokenProvider = tokenProvider;
        _userAuthenticationTokenRepository = userAuthenticationTokenRepository;
        _orderRepository = orderRepository;
    }
    #endregion

    public async Task<Result<CustomerSignInResponse>> Handle(
        CustomerSignInCommand request,
        CancellationToken cancellationToken)
    {
        var (response, failure) = await IsSignInSuccess(request);
        if (failure is not null) return Result.Failure<CustomerSignInResponse>(failure.Error);

        #region Đồng bộ giỏ hàng giữa guest và customer
        // var orderInfo = await _orderRepository.GetWaitingOrderByCustomerId(CustomerId.FromGuid(request.GuestId));
        // if(orderInfo is not null && orderInfo.OrderDetails is not null)
        // {

        // }
        #endregion

        //Tạo access token và 
        string jti = Ulid.NewUlid().ToGuid().ToString();
        string accessToken = _tokenProvider.GenerateAccessTokenForCustomer(
            UserId.FromUlid(response!.UserId),
            CustomerId.FromUlid(response.CustomerId),
            Email.FromString(response.Email),
            response.CustomerType,
            jti);

        string refreshToken = _tokenProvider.GenerateRefreshToken();

        // Save jti and refresh token to database
        var userAuthenticationToken = UserAuthenticationToken.NewUserAuthenticationToken(
            refreshToken,
            jti,
            DateTime.UtcNow.AddDays(30),
            UserId.FromUlid(response!.UserId));

        await _userAuthenticationTokenRepository.AddRefreshToken(userAuthenticationToken);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success(new CustomerSignInResponse(accessToken, refreshToken));
    }

    #region Private method
    private async Task<(CustomerAuthenticationResponse? response, Result<CustomerSignInResponse>? failure)> IsSignInSuccess(
        CustomerSignInCommand request)
    {
        var response = await _customerQueryServices.AuthenticateCustomer(
            Email.NewEmail(request.Email),
            PasswordHash.NewPasswordHash(request.Password.SHA256()));

        if (response is null) return (null, Result.Failure<CustomerSignInResponse>(CustomerError.LoginFailed()));

        return (response, null);
    }
    #endregion
}
