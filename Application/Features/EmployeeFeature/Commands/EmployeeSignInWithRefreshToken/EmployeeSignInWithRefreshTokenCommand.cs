using Application.Abstractions.Messaging;
using Application.DTOs.Employee;

namespace Application.Features.EmployeeFeature.Commands.EmployeeSignInWithRefreshToken;

public record EmployeeSignInWithRefreshTokenCommand(string RefreshToken) : ICommand<EmployeeSignInResponse>; 