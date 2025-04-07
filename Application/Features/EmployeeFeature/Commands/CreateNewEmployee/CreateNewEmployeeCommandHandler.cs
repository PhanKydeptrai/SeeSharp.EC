using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Application.Outbox;
using Application.Security;
using Domain.Entities.Employees;
using Domain.Entities.Users;
using Domain.IRepositories;
using Domain.IRepositories.Employees;
using Domain.IRepositories.Users;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.EmployeeEvents;
using NETCore.Encrypt.Extensions;
using SharedKernel;

namespace Application.Features.EmployeeFeature.Commands.CreateNewEmployee;

internal sealed class CreateNewEmployeeCommandHandler : ICommandHandler<CreateNewEmployeeCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutBoxMessageServices _outboxMessageServices;
    private readonly IEventBus _eventBus;

    public CreateNewEmployeeCommandHandler(
        IUserRepository userRepository,
        IEmployeeRepository employeeRepository,
        IUnitOfWork unitOfWork,
        ITokenProvider tokenProvider,
        IOutBoxMessageServices outboxMessageServices,
        IEventBus eventBus)
    {
        _userRepository = userRepository;
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
        _tokenProvider = tokenProvider;
        _outboxMessageServices = outboxMessageServices;
        _eventBus = eventBus;
    }

    public async Task<Result> Handle(CreateNewEmployeeCommand request, CancellationToken cancellationToken)
    {
        var randomPassword = _tokenProvider.GenerateRandomString(8);

        var user = User.NewUser(
            null, 
            UserName.NewUserName(request.UserName), 
            Email.NewEmail(request.Email),
            PhoneNumber.NewPhoneNumber(request.PhoneNumber),
            PasswordHash.FromString(randomPassword.SHA256()),
            request.DateOfBirth,
            string.Empty); //TODO: Add imageUrl

        var employee = Employee.NewEmployee(user.UserId);
        
        await _userRepository.AddUserToPostgreSQL(user);
        
        await _employeeRepository.AddEmployee(employee);

        var message = new SendDefaultPasswordToUserEvent(request.Email, randomPassword, Ulid.NewUlid().ToGuid());

        await OutboxMessageExtentions.InsertOutboxMessageAsync(message.MessageId, message, _outboxMessageServices);
        
        await _unitOfWork.SaveChangeAsync();

        await _eventBus.PublishAsync(message);
        return Result.Success();
    }
}