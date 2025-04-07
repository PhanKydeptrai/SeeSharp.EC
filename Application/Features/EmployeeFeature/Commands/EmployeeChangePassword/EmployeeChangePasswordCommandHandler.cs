using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Application.Outbox;
using Domain.Entities.Users;
using Domain.Entities.VerificationTokens;
using Domain.IRepositories;
using Domain.IRepositories.Users;
using Domain.IRepositories.VerificationTokens;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Errors;
using Domain.Utilities.Events.EmployeeEvents;
using NETCore.Encrypt.Extensions;
using SharedKernel;

namespace Application.Features.EmployeeFeature.Commands.EmployeeChangePassword;

internal sealed class EmployeeChangePasswordCommandHandler : ICommandHandler<EmployeeChangePasswordCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IEventBus _eventBus;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IVerificationTokenRepository _verificationTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public EmployeeChangePasswordCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IEventBus eventBus,
        IOutBoxMessageServices outBoxMessageServices,
        IVerificationTokenRepository verificationTokenRepository)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
        _outBoxMessageServices = outBoxMessageServices;
        _verificationTokenRepository = verificationTokenRepository;
    }

    public async Task<Result> Handle(EmployeeChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserFromPostgreSQL(UserId.FromGuid(request.userId));
        if (user is null)
        {
            return Result.Failure(EmployeeError.FailToChangePassword());
        }

        // Verify current password
        if (user.PasswordHash!.Value != request.currentPassword.SHA256())
        {
            return Result.Failure(EmployeeError.PasswordNotMatch());
        }

        // Verify if new password matches repeat password
        if (request.newPassword != request.repeatNewPassword)
        {
            return Result.Failure(EmployeeError.PasswordNotMatch());
        }

        var verificationToken = VerificationToken.NewVerificationToken(
            null,
            user.UserId,
            DateTime.UtcNow.AddHours(24));

        await _verificationTokenRepository.AddVerificationTokenToPostgreSQL(verificationToken);

        var message = new EmployeeChangePasswordEvent(
            user.UserId.Value, 
            verificationToken.VerificationTokenId.Value,
            user.Email!.Value,
            Ulid.NewUlid().ToGuid());

        await OutboxMessageExtentions.InsertOutboxMessageAsync(message.MessageId, message, _outBoxMessageServices);

        await _unitOfWork.SaveChangeAsync();

        await _eventBus.PublishAsync(message);

        return Result.Success();
    }
} 