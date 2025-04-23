using Application.Abstractions.Messaging;

namespace Application.Features.EmployeeFeature.Commands.EmployeeRevokeRefreshToken;

public record EmployeeRevokeRefreshTokenCommand(string jti) : ICommand; 