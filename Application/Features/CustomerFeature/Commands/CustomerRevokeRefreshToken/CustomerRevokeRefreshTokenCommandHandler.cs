using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.IRepositories;
using Domain.IRepositories.UserAuthenticationTokens;
using SharedKernel;

namespace Application.Features.CustomerFeature.Commands.CustomerRevokeRefreshToken;

internal sealed class CustomerRevokeRefreshTokenCommandHandler : ICommandHandler<CustomerRevokeRefreshTokenCommand>
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

    public async Task<Result> Handle(CustomerRevokeRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _unitOfWork.BeginMySQLTransaction();
        await _userAuthenticationTokenRepository.RemoveRefreshTokenFromMySQL(UserId.FromGuid(request.UserId));
        await _unitOfWork.SaveToMySQL();
        transaction.Commit();
        return Result.Success();
    }
}
