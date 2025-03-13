using Application.Abstractions.Messaging;

namespace Application.Features.CustomerFeature.Commands.CustomerResetPassword;

public record CustomerResetPasswordCommand(string Email) : ICommand;
