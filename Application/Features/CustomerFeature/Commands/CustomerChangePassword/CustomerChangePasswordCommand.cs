using Application.Abstractions.Messaging;

namespace Application.Features.CustomerFeature.Commands.CustomerChangePassword;

public record CustomerChangePasswordCommand(
    Guid userId, 
    string currentPassword,
    string newPassword,
    string repeatNewPassword) : ICommand;

public record CustomerChangePasswordRequest(
    string currentPassword,
    string newPassword,
    string repeatNewPassword);
