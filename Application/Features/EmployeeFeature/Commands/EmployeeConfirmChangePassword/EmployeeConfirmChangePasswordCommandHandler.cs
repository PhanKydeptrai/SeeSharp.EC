using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Application.Outbox;
using Domain.Entities.VerificationTokens;
using Domain.IRepositories;
using Domain.IRepositories.VerificationTokens;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Errors;
using Domain.Utilities.Events.EmployeeEvents;
using SharedKernel;

namespace Application.Features.EmployeeFeature.Commands.EmployeeConfirmChangePassword;

internal sealed class EmployeeConfirmChangePasswordCommandHandler : ICommandHandler<EmployeeConfirmChangePasswordCommand>
{
    private readonly IVerificationTokenRepository _verificationTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IEventBus _eventBus;

    public EmployeeConfirmChangePasswordCommandHandler(
        IVerificationTokenRepository verificationTokenRepository,
        IUnitOfWork unitOfWork,
        IOutBoxMessageServices outBoxMessageServices,
        IEventBus eventBus)
    {
        _verificationTokenRepository = verificationTokenRepository;
        _unitOfWork = unitOfWork;
        _outBoxMessageServices = outBoxMessageServices;
        _eventBus = eventBus;
    }

    public async Task<Result> Handle(
        EmployeeConfirmChangePasswordCommand request, 
        CancellationToken cancellationToken)
    {
        var token = await _verificationTokenRepository.GetVerificationTokenFromPostgreSQL(
            VerificationTokenId.FromGuid(request.verificationTokenId));
        
        if (token is null)
        {
            return Result.Failure(EmployeeError.NotFoundToken(request.verificationTokenId));
        }

        if (token.ExpiredDate < DateTime.UtcNow)
        {
            return Result.Failure(EmployeeError.TokenExpired(request.verificationTokenId));
        }

        var message = new EmployeeConfirmChangePasswordEvent(
            token.User!.Email!.Value,
            Ulid.NewUlid().ToGuid());

        await OutboxMessageExtentions.InsertOutboxMessageAsync(
            message.MessageId, message, _outBoxMessageServices);

        _verificationTokenRepository.RemoveVerificationTokenFrommPostgreSQL(token);

        await _unitOfWork.SaveChangesAsync();

        await _eventBus.PublishAsync(message);

        return Result.Success();
    }
} 