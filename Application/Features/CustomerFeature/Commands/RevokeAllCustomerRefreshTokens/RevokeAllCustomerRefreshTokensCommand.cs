using Application.Abstractions.Messaging;

namespace Application.Features.CustomerFeature.Commands.RevokeAllCustomerRefreshTokens;

public record RevokeAllCustomerRefreshTokensCommand(Guid userId) : ICommand;
