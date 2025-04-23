using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.IRepositories;
using Domain.IRepositories.UserAuthenticationTokens;
using SharedKernel;

namespace Application.Features.EmployeeFeature.Commands.RevokeAllEmployeeRefreshTokens;

internal sealed class RevokeAllEmployeeRefreshTokensCommandHandler
    : ICommandHandler<RevokeAllEmployeeRefreshTokensCommand>
{
    private readonly IUserAuthenticationTokenRepository _userAuthenticationTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public RevokeAllEmployeeRefreshTokensCommandHandler(
        IUnitOfWork unitOfWork,
        IUserAuthenticationTokenRepository userAuthenticationTokenRepository)
    {
        _unitOfWork = unitOfWork;
        _userAuthenticationTokenRepository = userAuthenticationTokenRepository;
    }

    public async Task<Result> Handle(
        RevokeAllEmployeeRefreshTokensCommand request, 
        CancellationToken cancellationToken)
    {
        await _userAuthenticationTokenRepository.RevokeAllTokenFromMySQLByUserId(
            UserId.FromGuid(request.userId));
            
        await _unitOfWork.SaveChangesAsync();
        
        return Result.Success();
    }
} 