using Application.Abstractions.Messaging;

namespace Application.Features.EmployeeFeature.Commands.EmployeeResetPassword;

public record EmployeeResetPasswordCommand(string Email) : ICommand; 