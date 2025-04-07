using Application.Abstractions.Messaging;

namespace Application.Features.EmployeeFeature.Commands.RevokeAllEmployeeRefreshTokens;

public record RevokeAllEmployeeRefreshTokensCommand(Guid userId) : ICommand; 