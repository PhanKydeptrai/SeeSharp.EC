using System.IdentityModel.Tokens.Jwt;

namespace API.Extentions;

internal static class TokenExtentions
{
    internal static string GetTokenFromHeader(HttpContext httpContext)
    {
        //lấy token
        string token = httpContext.Request.Headers["Authorization"]
            .FirstOrDefault()!
            .Substring("Bearer ".Length)
            .Trim();

        return token;
    }

    public static IDictionary<string, string> DecodeJwt(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwtToken = handler.ReadJwtToken(token);

        var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);

        return claims;
    }
}
