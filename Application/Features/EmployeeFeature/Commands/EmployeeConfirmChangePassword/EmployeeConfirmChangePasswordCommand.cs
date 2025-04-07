using Application.Abstractions.Messaging;

namespace Application.Features.EmployeeFeature.Commands.EmployeeConfirmChangePassword;

public record EmployeeConfirmChangePasswordCommand(Guid verificationTokenId) : ICommand; 