using Application.Abstractions.Messaging;
using Application.DTOs.Customer;
using Application.Security;
using Domain.Entities.UserAuthenticationTokens;
using Domain.IRepositories;
using Domain.IRepositories.UserAuthenticationTokens;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.CustomerFeature.Commands.CustomerSignInWithRefreshToken;

internal sealed class CustomerSignInWithRefreshTokenCommandHandler
    : ICommandHandler<CustomerSignInWithRefreshTokenCommand, CustomerSignInResponse>
{
    private readonly IUserAuthenticationTokenRepository _userAuthenticationTokenRepository;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUnitOfWork _unitOfWork;
    public CustomerSignInWithRefreshTokenCommandHandler(
        IUserAuthenticationTokenRepository userAuthenticationTokenRepository,
        IUnitOfWork unitOfWork,
        ITokenProvider tokenProvider)
    {
        _userAuthenticationTokenRepository = userAuthenticationTokenRepository;
        _unitOfWork = unitOfWork;
        _tokenProvider = tokenProvider;
    }

    public async Task<Result<CustomerSignInResponse>> Handle(
        CustomerSignInWithRefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var (userAuhenticationToken, failure) = await GetRefreshTokenFromMySQL(request);
        if (userAuhenticationToken is null)
        {
            return failure!;
        }

        string accessToken = _tokenProvider.GenerateJwtToken(
            userAuhenticationToken.UserId,
            userAuhenticationToken.User!.Email!,
            userAuhenticationToken.User.Customer!.CustomerType.ToString());

        string refreshToken = _tokenProvider.GenerateRefreshToken();
        userAuhenticationToken.BlackList();
        var response = new CustomerSignInResponse(accessToken, refreshToken);

        // Save jti and refresh token to database
        var newUserAuhenticationToken = UserAuthenticationToken.NewUserAuthenticationToken(
            response.refreshToken,
            string.Empty, //NOTE: Empty because just record jti when access token revoked
            DateTime.UtcNow.AddDays(30),
            userAuhenticationToken.UserId);

        await _userAuthenticationTokenRepository.AddRefreshTokenToMySQL(newUserAuhenticationToken);

        await _unitOfWork.SaveToMySQL();

        return Result.Success(response);
    }

    #region Private Methods
    private async Task<(UserAuthenticationToken? userAuthenticationToken, Result<CustomerSignInResponse>? result)> GetRefreshTokenFromMySQL(
        CustomerSignInWithRefreshTokenCommand request)
    {
        var user = await _userAuthenticationTokenRepository
            .GetAuthenticationTokenWithRefreshToken(request.RefreshToken);

        if (user is null)
        {
            return (null, Result.Failure<CustomerSignInResponse>(CustomerError.RefreshTokenNotFound()));
        }

        if(user.ExpiredTime < DateTime.UtcNow)
        {
            return (null, Result.Failure<CustomerSignInResponse>(CustomerError.RefreshTokenExpired()));
        }
        
        if (user.IsBlackList.Value)
        {
            return (null, Result.Failure<CustomerSignInResponse>(CustomerError.RefreshTokenIsBlackList()));
        }


        return (user, null);
    }
    #endregion

}
