using Application.Abstractions.Messaging;

namespace Application.Features.CustomerFeature.Commands.CustomerConfirmResetPassword;

public record CustomerConfirmResetPasswordCommand(Guid token) : ICommand;
