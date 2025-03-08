using Application.Abstractions.Messaging;

namespace Application.Features.CustomerFeature.Commands.CustomerConfirmChangePassword;

public record CustomerConfirmChangePasswordCommand(Guid TokenId) : ICommand;
