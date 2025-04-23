using Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;

namespace Application.Features.CustomerFeature.Commands.UpdateCustomerProfile;


public record UpdateCustomerProfileRequest(
    string UserName,
    string PhoneNumber,
    int Gender,
    string? DateOfBirth,
    IFormFile? ImageFile); 
    
public record UpdateCustomerProfileCommand(
    Guid UserId,
    string UserName,
    string PhoneNumber,
    int Gender,
    DateOnly? DateOfBirth,
    IFormFile? ImageFile) : ICommand;
