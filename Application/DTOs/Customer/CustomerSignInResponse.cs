namespace Application.DTOs.Customer;

public record CustomerSignInResponse(
    string accessToken,
    string refreshToken);
