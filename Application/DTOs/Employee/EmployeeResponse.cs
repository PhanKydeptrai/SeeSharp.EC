namespace Application.DTOs.Employee;

public record EmployeeResponse(
    Guid UserId,
    string UserName,
    DateTime? DateOfBirth,
    string? ImageUrl,
    string PhoneNumber,
    string Email,
    string Role,
    string UserStatus,
    DateTime CreatedAt); 