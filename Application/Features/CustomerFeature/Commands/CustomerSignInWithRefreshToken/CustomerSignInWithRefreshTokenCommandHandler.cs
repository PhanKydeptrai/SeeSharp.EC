// using Application.Abstractions.Messaging;
// using Application.DTOs.Customer;
// using Application.Security;
// using Domain.Entities.UserAuthenticationTokens;
// using Domain.IRepositories;
// using Domain.Utilities.Errors;
// using SharedKernel;

// namespace Application.Features.CustomerFeature.Commands.CustomerSignInWithRefreshToken;

// internal sealed class CustomerSignInWithRefreshTokenCommandHandler
//     : ICommandHandler<CustomerSignInWithRefreshTokenCommand, CustomerSignInResponse>
// {
//     private readonly ITokenProvider _tokenProvider;
//     private readonly IUnitOfWork _unitOfWork;
//     public CustomerSignInWithRefreshTokenCommandHandler(
//         IUnitOfWork unitOfWork,
//         ITokenProvider tokenProvider)
//     {
//         _unitOfWork = unitOfWork;
//         _tokenProvider = tokenProvider;
//     }

//     public async Task<Result<CustomerSignInResponse>> Handle(
//         CustomerSignInWithRefreshTokenCommand request,
//         CancellationToken cancellationToken)
//     {
//         // Get refresh token from database and set it to blacklist
//         var (userAuhenticationToken, failure) = await GetRefreshTokenFromMySQL(request);
//         if (userAuhenticationToken is null)
//         {
//             return failure!;
//         }

//         userAuhenticationToken.BlackList();

//         // Generate token
//         string jti = Ulid.NewUlid().ToGuid().ToString();
//         string accessToken = _tokenProvider.GenerateAccessToken(
//             userAuhenticationToken.UserId,
//             userAuhenticationToken.User!.Customer!.CustomerId,
//             userAuhenticationToken.User!.Email!,
//             userAuhenticationToken.User.Customer!.CustomerType.ToString(),
//             jti);

//         string refreshToken = _tokenProvider.GenerateRefreshToken();
        
//         var response = new CustomerSignInResponse(accessToken, refreshToken);

//         // Save jti and refresh token to database
//         var newUserAuhenticationToken = UserAuthenticationToken.NewUserAuthenticationToken(
//             response.refreshToken,
//             jti,
//             DateTime.UtcNow.AddDays(30),
//             userAuhenticationToken.UserId);

//         await _userAuthenticationTokenRepository.AddRefreshTokenToMySQL(newUserAuhenticationToken);

//         await _unitOfWork.SaveChangeAsync();

//         return Result.Success(response);
//     }

//     #region Private Methods
//     private async Task<(UserAuthenticationToken? userAuthenticationToken, Result<CustomerSignInResponse>? result)> GetRefreshTokenFromMySQL(
//         CustomerSignInWithRefreshTokenCommand request)
//     {
//         var user = await _userAuthenticationTokenRepository
//             .GetAuthenticationTokenWithRefreshToken(request.RefreshToken);

//         if (user is null || user.ExpiredTime < DateTime.UtcNow || user.IsBlackList.Value)
//         {
//             return (null, Result.Failure<CustomerSignInResponse>(CustomerError.RefreshTokenInvalid()));
//         }

//         return (user, null);
//     }
//     #endregion

// }
