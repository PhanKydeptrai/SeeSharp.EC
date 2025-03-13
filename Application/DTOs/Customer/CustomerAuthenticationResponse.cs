namespace Application.DTOs.Customer;

public record CustomerAuthenticationResponse(
    Ulid UserId,
    Ulid CustomerId,
    string Email,
    string UserStatus,
    string CustomerType);
