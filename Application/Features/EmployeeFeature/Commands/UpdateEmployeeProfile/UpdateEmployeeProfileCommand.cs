using Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;

namespace Application.Features.EmployeeFeature.Commands.UpdateEmployeeProfile;

public record UpdateEmployeeProfileCommand(
    Guid UserId,
    string UserName,
    string PhoneNumber,
    DateOnly? DateOfBirth,
    IFormFile? ImageFile) : ICommand;