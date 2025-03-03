using Domain.Entities.Users;
using Microsoft.AspNetCore.Http;

namespace Application.Security;

public interface ITokenProvider
{
    string GenerateJwtToken(UserId userId, Email email, string role);
    string GenerateRefreshToken();
    string GetTokenFromHeader(HttpContext header);
}