using Application.Abstractions.Messaging;

namespace Application.Features.EmployeeFeature.Commands.EmployeeChangePassword;

public record EmployeeChangePasswordCommand(
    Guid userId, 
    string currentPassword,
    string newPassword,
    string repeatNewPassword) : ICommand;

public record EmployeeChangePasswordRequest(
    string currentPassword,
    string newPassword,
    string repeatNewPassword); 