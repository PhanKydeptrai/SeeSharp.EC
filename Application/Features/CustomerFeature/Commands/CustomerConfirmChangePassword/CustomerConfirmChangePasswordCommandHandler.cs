using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Application.Outbox;
using Domain.Entities.Users;
using Domain.Entities.VerificationTokens;
using Domain.IRepositories;
using Domain.IRepositories.VerificationTokens;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Errors;
using Domain.Utilities.Events.CustomerEvents;
using SharedKernel;

namespace Application.Features.CustomerFeature.Commands.CustomerConfirmChangePassword;

internal sealed class CustomerConfirmChangePasswordCommandHandler : ICommandHandler<CustomerConfirmChangePasswordCommand>
{
    //NOTE:
    /*
    - Get token from database
    - Check token is valid
    - Update password
    - Publish event to update password on other database.
    - Send email to user
    */

    private readonly IVerificationTokenRepository _verificationTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutBoxMessageServices _outboxMessageServices;
    private readonly IEventBus _eventBus;

    public CustomerConfirmChangePasswordCommandHandler(
        IVerificationTokenRepository verificationTokenRepository,
        IUnitOfWork unitOfWork,
        IEventBus eventBus,
        IOutBoxMessageServices outboxMessageServices)
    {
        _verificationTokenRepository = verificationTokenRepository;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
        _outboxMessageServices = outboxMessageServices;
    }

    public async Task<Result> Handle(
        CustomerConfirmChangePasswordCommand request, 
        CancellationToken cancellationToken)
    {
        var verificationToken = await _verificationTokenRepository.GetVerificationTokenFromMySQL(
            VerificationTokenId.FromGuid(request.TokenId));
        
        if (verificationToken is null || verificationToken.User is null || verificationToken.ExpiredDate < DateTime.UtcNow)
        {
            return Result.Failure(CustomerError.VerificationTokenInvalid(VerificationTokenId.FromGuid(request.TokenId)));
        }

        verificationToken.User.ChangePassword(
            PasswordHash.FromString(verificationToken.Temporary!));

        var message = new CustomerConfirmChangePasswordEvent(
            verificationToken.User.UserId,
            verificationToken.User.Email!.Value,
            verificationToken.Temporary!,
            Ulid.NewUlid().ToGuid());

        await OutboxMessageExtentions.InsertOutboxMessageAsync(
            message.MessageId, 
            message, _outboxMessageServices);

        await _unitOfWork.SaveToMySQL();
        
        // Publish event
        await _eventBus.PublishAsync(message);
        return Result.Success();
    }
}
