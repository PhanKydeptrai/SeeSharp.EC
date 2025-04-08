using Application.Abstractions.Messaging;
using Application.DTOs.Employee;
using Application.IServices;
using Application.Security;
using Domain.Entities.Employees;
using Domain.Entities.UserAuthenticationTokens;
using Domain.Entities.Users;
using Domain.IRepositories;
using Domain.IRepositories.UserAuthenticationTokens;
using Domain.Utilities.Errors;
using NETCore.Encrypt.Extensions;
using SharedKernel;

namespace Application.Features.EmployeeFeature.Commands.EmployeeSignIn;

internal sealed class EmployeeSignInCommandHandler : ICommandHandler<EmployeeSignInCommand, EmployeeSignInResponse>
{
    private readonly IEmployeeQueryServices _employeeQueryServices;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUserAuthenticationTokenRepository _userAuthenticationTokenRepository;
    public EmployeeSignInCommandHandler(
        IEmployeeQueryServices employeeQueryServices,
        IUnitOfWork unitOfWork,
        ITokenProvider tokenProvider,
        IUserAuthenticationTokenRepository userAuthenticationTokenRepository)
    {
        _employeeQueryServices = employeeQueryServices;
        _unitOfWork = unitOfWork;
        _tokenProvider = tokenProvider;
        _userAuthenticationTokenRepository = userAuthenticationTokenRepository;
    }

    public async Task<Result<EmployeeSignInResponse>> Handle(
        EmployeeSignInCommand request, 
        CancellationToken cancellationToken)
    {
        var (response, failure) = await IsSignInSuccess(request);

        if (failure is not null) return Result.Failure<EmployeeSignInResponse>(failure.Error);

        //Tạo access token và 
        string jti = Ulid.NewUlid().ToGuid().ToString();
        string accessToken = _tokenProvider.GenerateAccessTokenForEmployee(
            UserId.FromUlid(response!.UserId),
            EmployeeId.FromUlid(response.EmployeeId),
            Email.FromString(response.Email),
            response.Role,
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

        return Result.Success(new EmployeeSignInResponse(accessToken, refreshToken));
    }

    private async Task<(EmployeeAuthenticationResponse? response, Result<EmployeeAuthenticationResponse>? failure)> IsSignInSuccess(
        EmployeeSignInCommand request)
    {
        var response = await _employeeQueryServices.AuthenticateEmployee(
            Email.NewEmail(request.Email),
            PasswordHash.NewPasswordHash(request.Password.SHA256()));

        if (response is null) return (null, Result.Failure<EmployeeAuthenticationResponse>(EmployeeError.LoginFailed()));

        return (response, null);
    }
}
