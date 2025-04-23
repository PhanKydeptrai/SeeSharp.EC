namespace Application.DTOs.Employee;

public record EmployeeAuthenticationResponse(
    Ulid UserId,
    Ulid EmployeeId,
    string Email,
    string UserStatus,
    string Role);
