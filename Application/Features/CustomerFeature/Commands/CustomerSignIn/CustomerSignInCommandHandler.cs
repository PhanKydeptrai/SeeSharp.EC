// using Application.Abstractions.Messaging;
// using Application.DTOs.Customer;
// using Application.IServices;
// using Application.Security;
// using Domain.Entities.Customers;
// using Domain.Entities.UserAuthenticationTokens;
// using Domain.Entities.Users;
// using Domain.IRepositories;
// using Domain.IRepositories.UserAuthenticationTokens;
// using Domain.Utilities.Errors;
// using NETCore.Encrypt.Extensions;
// using SharedKernel;

// namespace Application.Features.CustomerFeature.Commands.CustomerSignIn;

// internal sealed class CustomerSignInCommandHandler : ICommandHandler<CustomerSignInCommand, CustomerSignInResponse>
// {
//     #region Dependencies
//     private readonly ICustomerQueryServices _customerQueryServices;
//     private readonly ITokenProvider _tokenProvider;
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly IUserAuthenticationTokenRepository _userAuthenticationTokenRepository;
//     public CustomerSignInCommandHandler(
//         ITokenProvider tokenProvider,
//         IUnitOfWork unitOfWork,
//         ICustomerQueryServices customerQueryServices,
//         IUserAuthenticationTokenRepository userAuthenticationTokenRepository)
//     {
//         _tokenProvider = tokenProvider;
//         _unitOfWork = unitOfWork;
//         _customerQueryServices = customerQueryServices;
//         _userAuthenticationTokenRepository = userAuthenticationTokenRepository;
//     }
//     #endregion

//     public async Task<Result<CustomerSignInResponse>> Handle(
//         CustomerSignInCommand request,
//         CancellationToken cancellationToken)
//     {
//         var (response, failure) = await IsSignInSuccess(request);
//         if (failure is not null) return failure;

//         //Generate token
//         string jti = Ulid.NewUlid().ToGuid().ToString();
//         string accessToken = _tokenProvider.GenerateAccessToken(
//             UserId.FromUlid(response!.UserId),
//             CustomerId.FromUlid(response.CustomerId),
//             Email.FromString(response.Email),
//             response.CustomerType,
//             jti);

//         string refreshToken = _tokenProvider.GenerateRefreshToken();

//         // Save jti and refresh token to database
//         var userAuthenticationToken = UserAuthenticationToken.NewUserAuthenticationToken(
//             refreshToken,
//             jti,
//             DateTime.UtcNow.AddDays(30),
//             UserId.FromUlid(response!.UserId));

//         await _userAuthenticationTokenRepository.AddRefreshTokenToMySQL(userAuthenticationToken);
//         await _unitOfWork.SaveChangeAsync();
        
//         //Success
//         return Result.Success(new CustomerSignInResponse(accessToken, refreshToken));
//     }

//     #region Private method
//     private async Task<(CustomerAuthenticationResponse? response, Result<CustomerSignInResponse>? failure)> IsSignInSuccess(
//         CustomerSignInCommand request)
//     {
//         var response = await _customerQueryServices.AuthenticateCustomer(
//             Email.NewEmail(request.Email),
//             PasswordHash.NewPasswordHash(request.Password.SHA256()));

//         if (response is null)
//         {
//             return (null, Result.Failure<CustomerSignInResponse>(CustomerError.LoginFailed()));
//         }

//         return (response, null);
//     }
//     #endregion
// }
