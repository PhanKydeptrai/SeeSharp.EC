namespace Application.DTOs.Employee;

public record EmployeeSignInResponse(
    string accessToken,
    string refreshToken);