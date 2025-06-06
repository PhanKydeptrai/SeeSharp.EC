using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Application.Outbox;
using Application.Security;
using Domain.Entities.Users;
using Domain.Entities.VerificationTokens;
using Domain.IRepositories;
using Domain.IRepositories.UserAuthenticationTokens;
using Domain.IRepositories.VerificationTokens;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Errors;
using Domain.Utilities.Events.CustomerEvents;
using NETCore.Encrypt.Extensions;
using SharedKernel;

namespace Application.Features.CustomerFeature.Commands.CustomerConfirmResetPassword;

internal sealed class CustomerConfirmResetPasswordCommandHandler 
    : ICommandHandler<CustomerConfirmResetPasswordCommand>
{
    private readonly IVerificationTokenRepository _verificationTokenRepository;
    private readonly IUserAuthenticationTokenRepository _userAuthenticationTokenRepository;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenProvider _tokenProvider;
    private readonly IEventBus _eventBus;
    public CustomerConfirmResetPasswordCommandHandler(
        IUnitOfWork unitOfWork,
        IVerificationTokenRepository verificationTokenRepository,
        IOutBoxMessageServices outBoxMessageServices,
        IEventBus eventBus,
        ITokenProvider tokenProvider,
        IUserAuthenticationTokenRepository userAuthenticationTokenRepository)
    {
        _unitOfWork = unitOfWork;
        _verificationTokenRepository = verificationTokenRepository;
        _outBoxMessageServices = outBoxMessageServices;
        _eventBus = eventBus;
        _tokenProvider = tokenProvider;
        _userAuthenticationTokenRepository = userAuthenticationTokenRepository;
    }

    public async Task<Result> Handle(
        CustomerConfirmResetPasswordCommand request, 
        CancellationToken cancellationToken)
    {
        // Start a transaction
        using var transaction = await _unitOfWork.BeginPostgreSQLTransaction();
        
        var user = await _verificationTokenRepository.GetVerificationTokenFromPostgreSQL(
            VerificationTokenId.FromGuid(request.token));
        
        if(user is null) 
        {
            transaction.Rollback();
            return Result.Failure(CustomerError.InValidToken());
        }
        
        string randomPass = _tokenProvider.GenerateRandomString(8);

        user.User!.ChangePassword(PasswordHash.FromString(randomPass.SHA256()));
        
        //Create message and publish
        var message = new CustomerResetPasswordEvent(
            user!.UserId,
            user!.User.Email!.Value, 
            randomPass, 
            Ulid.NewUlid().ToGuid());
        
        await OutboxMessageExtentions.InsertOutboxMessageAsync(
            message.MessageId, message, _outBoxMessageServices);
        
        _verificationTokenRepository.RemoveVerificationTokenFrommPostgreSQL(user);

        // Revoke all existing authentication tokens for this user
        await _userAuthenticationTokenRepository.RevokeAllTokenFromMySQLByUserId(user.UserId);
        
        await _unitOfWork.SaveChangesAsync();
        
        // Commit the transaction if everything succeeds
        transaction.Commit();
        
        await _eventBus.PublishAsync(message);

        return Result.Success();
    }
}
