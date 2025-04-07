using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Application.IServices;
using Application.Outbox;
using Domain.Entities.Users;
using Domain.Entities.VerificationTokens;
using Domain.IRepositories;
using Domain.IRepositories.VerificationTokens;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Errors;
using Domain.Utilities.Events.EmployeeEvents;
using SharedKernel;

namespace Application.Features.EmployeeFeature.Commands.EmployeeResetPassword;

internal sealed class EmployeeResetPasswordCommandHandler : ICommandHandler<EmployeeResetPasswordCommand>
{
    private readonly IVerificationTokenRepository _verificationTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmployeeQueryServices _employeeQueryServices;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IEventBus _eventBus;
    
    public EmployeeResetPasswordCommandHandler(
        IVerificationTokenRepository verificationTokenRepository,
        IUnitOfWork unitOfWork,
        IOutBoxMessageServices outBoxMessageServices,
        IEventBus eventBus,
        IEmployeeQueryServices employeeQueryServices)
    {
        _verificationTokenRepository = verificationTokenRepository;
        _unitOfWork = unitOfWork;
        _outBoxMessageServices = outBoxMessageServices;
        _eventBus = eventBus;
        _employeeQueryServices = employeeQueryServices;
    }

    public async Task<Result> Handle(EmployeeResetPasswordCommand request, CancellationToken cancellationToken)
    {
        //Check email is valid
        var employee = await _employeeQueryServices.GetEmployeeByEmail(Email.NewEmail(request.Email));
        if (employee is null) return Result.Failure(EmployeeError.InValidInformation());

        var verificationToken = VerificationToken.NewVerificationToken(
            null,
            employee.UserId,
            DateTime.UtcNow.AddHours(24));

        await _verificationTokenRepository.AddVerificationTokenToPostgreSQL(verificationToken);

        //Create send password reset message and outbox message
        var message = new EmployeeResetPasswordEmailSendEvent(
            employee.UserId.Value,
            verificationToken.VerificationTokenId.Value,
            request.Email,
            Ulid.NewUlid().ToGuid());
        
        await OutboxMessageExtentions.InsertOutboxMessageAsync(message.MessageId, message, _outBoxMessageServices);

        await _unitOfWork.SaveChangeAsync();

        await _eventBus.PublishAsync(message);

        return Result.Success();
    }
} 