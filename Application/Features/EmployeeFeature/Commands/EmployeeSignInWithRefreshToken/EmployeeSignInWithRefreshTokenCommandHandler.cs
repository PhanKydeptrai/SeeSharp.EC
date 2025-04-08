using Application.Abstractions.Messaging;
using Application.DTOs.Employee;
using Application.Security;
using Domain.Entities.UserAuthenticationTokens;
using Domain.IRepositories;
using Domain.IRepositories.UserAuthenticationTokens;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.EmployeeFeature.Commands.EmployeeSignInWithRefreshToken;

internal sealed class EmployeeSignInWithRefreshTokenCommandHandler
    : ICommandHandler<EmployeeSignInWithRefreshTokenCommand, EmployeeSignInResponse>
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IUserAuthenticationTokenRepository _userAuthenticationTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    public EmployeeSignInWithRefreshTokenCommandHandler(
        IUnitOfWork unitOfWork,
        ITokenProvider tokenProvider,
        IUserAuthenticationTokenRepository userAuthenticationTokenRepository)
    {
        _unitOfWork = unitOfWork;
        _tokenProvider = tokenProvider;
        _userAuthenticationTokenRepository = userAuthenticationTokenRepository;
    }

    public async Task<Result<EmployeeSignInResponse>> Handle(
        EmployeeSignInWithRefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        // Get refresh token from database and set it to blacklist
        var (userAuhenticationToken, failure) = await GetRefreshToken(request);
        if (userAuhenticationToken is null)
        {
            return failure!;
        }

        userAuhenticationToken.BlackList();

        // Generate token
        string jti = Ulid.NewUlid().ToGuid().ToString();
        string accessToken = _tokenProvider.GenerateAccessTokenForEmployee(
            userAuhenticationToken.UserId,
            userAuhenticationToken.User!.Employee!.EmployeeId,
            userAuhenticationToken.User!.Email!,
            userAuhenticationToken.User.Employee!.Role.ToString(),
            jti);

        string refreshToken = _tokenProvider.GenerateRefreshToken();
        
        var response = new EmployeeSignInResponse(accessToken, refreshToken);

        // Save jti and refresh token to database
        var newUserAuhenticationToken = UserAuthenticationToken.NewUserAuthenticationToken(
            response.refreshToken,
            jti,
            DateTime.UtcNow.AddDays(30),
            userAuhenticationToken.UserId);

        await _userAuthenticationTokenRepository.AddRefreshToken(newUserAuhenticationToken);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success(response);
    }

    #region Private Methods
    private async Task<(UserAuthenticationToken? userAuthenticationToken, Result<EmployeeSignInResponse>? result)> GetRefreshToken(
        EmployeeSignInWithRefreshTokenCommand request)
    {
        var user = await _userAuthenticationTokenRepository
            .GetAuthenticationTokenWithRefreshToken(request.RefreshToken);

        if (user is null || user.ExpiredTime < DateTime.UtcNow || user.IsBlackList.Value)
        {
            return (null, Result.Failure<EmployeeSignInResponse>(EmployeeError.RefreshTokenInvalid()));
        }

        return (user, null);
    }
    #endregion
} 