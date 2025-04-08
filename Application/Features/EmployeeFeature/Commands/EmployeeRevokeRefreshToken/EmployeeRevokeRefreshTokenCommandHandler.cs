using Application.Abstractions.Messaging;
using Domain.IRepositories;
using Domain.IRepositories.UserAuthenticationTokens;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.EmployeeFeature.Commands.EmployeeRevokeRefreshToken;

internal sealed class EmployeeRevokeRefreshTokenCommandHandler 
    : ICommandHandler<EmployeeRevokeRefreshTokenCommand>
{
    private readonly IUserAuthenticationTokenRepository _userAuthenticationTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public EmployeeRevokeRefreshTokenCommandHandler(
        IUnitOfWork unitOfWork,
        IUserAuthenticationTokenRepository userAuthenticationTokenRepository)
    {
        _unitOfWork = unitOfWork;
        _userAuthenticationTokenRepository = userAuthenticationTokenRepository;
    }

    public async Task<Result> Handle(
        EmployeeRevokeRefreshTokenCommand request, 
        CancellationToken cancellationToken)
    {
        var userAuthenticationToken = await _userAuthenticationTokenRepository
            .GetRefreshTokenFromMySQLByJti(request.jti);

        if (userAuthenticationToken is null)
            return Result.Failure(EmployeeError.RefreshTokenInvalid());
        
        userAuthenticationToken.BlackList();
        await _unitOfWork.SaveChangesAsync();
        
        return Result.Success();
    }
} 