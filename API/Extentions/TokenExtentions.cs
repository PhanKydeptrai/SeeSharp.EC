using System.IdentityModel.Tokens.Jwt;

namespace API.Extentions;
internal static class TokenExtentions
{
    internal static string? GetTokenFromHeader(HttpContext httpContext)
    {
        var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            return null;
        }

        return authHeader.Substring("Bearer ".Length).Trim();
    }

    public static IDictionary<string, string> DecodeJwt(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwtToken = handler.ReadJwtToken(token);

        var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);

        return claims;
    }

    public const string CustomerId = "CustomerId";
}
