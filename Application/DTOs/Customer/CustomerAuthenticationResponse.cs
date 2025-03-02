namespace Application.DTOs.Customer;

public record CustomerAuthenticationResponse(
    Ulid UserId,
    string Email,
    string UserStatus,
    string CustomerType);
