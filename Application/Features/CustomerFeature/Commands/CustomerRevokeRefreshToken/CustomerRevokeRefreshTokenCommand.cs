using Application.Abstractions.Messaging;

namespace Application.Features.CustomerFeature.Commands.CustomerRevokeRefreshToken;

public record CustomerRevokeRefreshTokenCommand(string jti) : ICommand;