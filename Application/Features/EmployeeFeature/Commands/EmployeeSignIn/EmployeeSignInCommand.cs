using Application.Abstractions.Messaging;
using Application.DTOs.Employee;

namespace Application.Features.EmployeeFeature.Commands.EmployeeSignIn;

public record EmployeeSignInCommand(string Email, string Password) : ICommand<EmployeeSignInResponse>;