using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Application.Outbox;
using Domain.Entities.Users;
using Domain.Entities.VerificationTokens;
using Domain.IRepositories;
using Domain.IRepositories.UserAuthenticationTokens;
using Domain.IRepositories.Users;
using Domain.IRepositories.VerificationTokens;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Errors;
using Domain.Utilities.Events.CustomerEvents;
using NETCore.Encrypt.Extensions;
using SharedKernel;

namespace Application.Features.CustomerFeature.Commands.CustomerChangePassword;

internal sealed class CustomerChangePasswordCommandHandler : ICommandHandler<CustomerChangePasswordCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IEventBus _eventBus;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IVerificationTokenRepository _verificationTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserAuthenticationTokenRepository _userAuthenticationTokenRepository;
    public CustomerChangePasswordCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IEventBus eventBus,
        IOutBoxMessageServices outBoxMessageServices,
        IUserAuthenticationTokenRepository userAuthenticationTokenRepository,
        IVerificationTokenRepository verificationTokenRepository)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
        _outBoxMessageServices = outBoxMessageServices;
        _userAuthenticationTokenRepository = userAuthenticationTokenRepository;
        _verificationTokenRepository = verificationTokenRepository;
    }

    public async Task<Result> Handle(CustomerChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserFromMySQL(UserId.FromGuid(request.userId));
        if (user is null)
        {
            return Result.Failure(CustomerError.FailToChangePassword());
        }

        //NOTE: Xử lý trường hợp login google
        //-----------------------------------
        if (user.PasswordHash!.Value != request.currentPassword.SHA256())
        {
            return Result.Failure(CustomerError.PasswordNotMatch());
        }

        //Create verify token
        var token = VerificationToken.NewVerificationToken(
            request.newPassword.SHA256(),
            user.UserId,
            DateTime.UtcNow.AddMinutes(15));

        await _verificationTokenRepository.AddVerificationTokenToMySQL(token);

        //Create event
        var message = new CustomerChangePasswordEvent(
            user.UserId,
            token.VerificationTokenId, 
            user.Email!.Value,
            Ulid.NewUlid().ToGuid());

        await OutboxMessageExtentions.InsertOutboxMessageAsync(
            message.MessageId, 
            message, _outBoxMessageServices);

        await _userAuthenticationTokenRepository.RevokeAllTokenFromMySQLByUserId(
            UserId.FromGuid(request.userId));

        await _unitOfWork.SaveToMySQL();

        //Publish event
        await _eventBus.PublishAsync(message);
        return Result.Success();
    }
}
