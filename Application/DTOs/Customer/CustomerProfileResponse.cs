namespace Application.DTOs.Customer;

public record CustomerProfileResponse(
    Guid UserId,
    string UserName,
    DateTime? DateOfBirth,
    string? ImageUrl,
    string Gender,
    string PhoneNumber,
    string Email,
    string CustomerType,
    string UserStatus);