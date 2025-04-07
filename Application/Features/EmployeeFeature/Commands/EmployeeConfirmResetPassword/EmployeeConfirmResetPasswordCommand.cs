using Application.Abstractions.Messaging;

namespace Application.Features.EmployeeFeature.Commands.EmployeeConfirmResetPassword;

public record EmployeeConfirmResetPasswordCommand(Guid token) : ICommand; 