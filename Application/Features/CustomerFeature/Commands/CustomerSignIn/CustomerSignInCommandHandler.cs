using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Application.DTOs.Customer;
using Application.IServices;
using Application.Security;
using Domain.Entities.UserAuthenticationTokens;
using Domain.Entities.Users;
using Domain.IRepositories;
using Domain.IRepositories.UserAuthenticationTokens;
using Domain.Utilities.Errors;
using NETCore.Encrypt.Extensions;
using SharedKernel;

namespace Application.Features.CustomerFeature.Commands.CustomerSignIn;

internal sealed class CustomerSignInCommandHandler : ICommandHandler<CustomerSignInCommand, CustomerSignInResponse>
{
    #region Dependencies
    private readonly ICustomerQueryServices _customerQueryServices;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserAuthenticationTokenRepository _userAuthenticationTokenRepository;
    private readonly IEventBus _eventBus;
    public CustomerSignInCommandHandler(
        ITokenProvider tokenProvider,
        IUnitOfWork unitOfWork,
        ICustomerQueryServices customerQueryServices,
        IEventBus eventBus,
        IUserAuthenticationTokenRepository userAuthenticationTokenRepository)
    {
        _tokenProvider = tokenProvider;
        _unitOfWork = unitOfWork;
        _customerQueryServices = customerQueryServices;
        _eventBus = eventBus;
        _userAuthenticationTokenRepository = userAuthenticationTokenRepository;
    }
    #endregion

    public async Task<Result<CustomerSignInResponse>> Handle(
        CustomerSignInCommand request,
        CancellationToken cancellationToken)
    {
        var (response, failure) = await IsSignInSuccess(request);
        if (failure is not null) return failure;
        var signinResponse = CreateResponse(response!);

        //Save access token and refresh token to database
        // var accessToken = UserAuthenticationToken.NewUserAuthenticationToken(
        //     signinResponse.accessToken,
        //     TokenType.AccessToken,
        //     DateTime.UtcNow.AddMinutes(15),
        //     UserId.FromUlid(response!.UserId));

        var refreshToken = UserAuthenticationToken.NewUserAuthenticationToken(
            signinResponse.refreshToken,
            TokenType.RefreshToken,
            DateTime.UtcNow.AddDays(30),
            UserId.FromUlid(response!.UserId));

        await _userAuthenticationTokenRepository.AddRefreshTokenToMySQL(refreshToken);
        await _unitOfWork.SaveToMySQL();
        return Result.Success<CustomerSignInResponse>(signinResponse);
    }

    #region Private method
    private CustomerSignInResponse CreateResponse(CustomerAuthenticationResponse response)
    {
        string accessToken = _tokenProvider.GenerateJwtToken(
            UserId.FromUlid(response.UserId),
            Email.FromString(response.Email),
            response.CustomerType);

        string refreshToken = _tokenProvider.GenerateRefreshToken();
        return new CustomerSignInResponse(accessToken, refreshToken);
    }

    private async Task<(CustomerAuthenticationResponse? response, Result<CustomerSignInResponse>? failure)> IsSignInSuccess(
        CustomerSignInCommand request)
    {
        var response = await _customerQueryServices.IsCustomerSignInSuccess(
            Email.NewEmail(request.Email),
            PasswordHash.NewPasswordHash(request.Password.SHA256()));

        if (response is null)
        {
            return (null, Result.Failure<CustomerSignInResponse>(CustomerError.LoginFailed()));
        }

        return (response, null);
    }
    #endregion
}
