using Domain.Entities.Customers;
using Domain.Entities.Users;

namespace Application.Security;

public interface ITokenProvider
{
    string GenerateAccessToken(UserId userId, CustomerId customerId, Email email, string role, string jti);
    string GenerateRefreshToken();
    string GenerateRandomString(int length);
}