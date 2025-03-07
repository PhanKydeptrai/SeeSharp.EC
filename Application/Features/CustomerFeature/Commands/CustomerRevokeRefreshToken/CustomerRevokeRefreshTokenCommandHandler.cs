using Application.Abstractions.Messaging;
using Domain.IRepositories;
using Domain.IRepositories.UserAuthenticationTokens;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.CustomerFeature.Commands.CustomerRevokeRefreshToken;

internal sealed class CustomerRevokeRefreshTokenCommandHandler 
    : ICommandHandler<CustomerRevokeRefreshTokenCommand>
{
    private readonly IUserAuthenticationTokenRepository _userAuthenticationTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CustomerRevokeRefreshTokenCommandHandler(
        IUnitOfWork unitOfWork,
        IUserAuthenticationTokenRepository userAuthenticationTokenRepository)
    {
        _unitOfWork = unitOfWork;
        _userAuthenticationTokenRepository = userAuthenticationTokenRepository;
    }

    public async Task<Result> Handle(
        CustomerRevokeRefreshTokenCommand request, 
        CancellationToken cancellationToken)
    {
        
        var userAuthenticationToken = await _userAuthenticationTokenRepository
            .GetRefreshTokenFromMySQLByJti(request.jti);

        if (userAuthenticationToken is null)
            return Result.Failure(CustomerError.RefreshTokenInvalid());
        
        userAuthenticationToken.BlackList();
        await _unitOfWork.SaveToMySQL();
        
        return Result.Success();
    }
}
