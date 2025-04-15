using Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;

namespace Application.Features.EmployeeFeature.Commands.CreateNewEmployee;

public record CreateNewEmployeeCommand(
    string UserName,
    string Email,
    string PhoneNumber,
    DateOnly? DateOfBirth,
    IFormFile? ImageFile) : ICommand;