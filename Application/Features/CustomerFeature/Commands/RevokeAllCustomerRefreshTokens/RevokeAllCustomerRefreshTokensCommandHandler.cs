// using Application.Abstractions.Messaging;
// using Domain.Entities.Users;
// using Domain.IRepositories;
// using Domain.IRepositories.UserAuthenticationTokens;
// using SharedKernel;

// namespace Application.Features.CustomerFeature.Commands.RevokeAllCustomerRefreshTokens;

// internal sealed class RevokeAllCustomerRefreshTokensCommandHandler 
//     : ICommandHandler<RevokeAllCustomerRefreshTokensCommand>
// {
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly IUserAuthenticationTokenRepository _userAuthenticationTokenRepository;
//     public RevokeAllCustomerRefreshTokensCommandHandler(
//         IUnitOfWork unitOfWork, 
//         IUserAuthenticationTokenRepository userAuthenticationTokenRepository)
//     {
//         _unitOfWork = unitOfWork;
//         _userAuthenticationTokenRepository = userAuthenticationTokenRepository;
//     }

//     public async Task<Result> Handle(
//         RevokeAllCustomerRefreshTokensCommand request, 
//         CancellationToken cancellationToken)
//     {
//         using var transaction = await _unitOfWork.BeginPostgreSQLTransaction();
//         await _userAuthenticationTokenRepository.RevokeAllTokenFromMySQLByUserId(
//             UserId.FromGuid(request.userId));
//         transaction.Commit();

//         return Result.Success();
//     }
// }
