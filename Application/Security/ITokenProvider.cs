using Domain.Entities.Users;
using Microsoft.AspNetCore.Http;

namespace Application.Security;

public interface ITokenProvider
{
    string GenerateAccessToken(UserId userId, Email email, string role, string jti);
    string GenerateRefreshToken();
    string GetTokenFromHeader(HttpContext header);
}