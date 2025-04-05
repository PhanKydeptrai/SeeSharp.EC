using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Security;
using Domain.Entities.Customers;
using Domain.Entities.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security;

public class TokenProvider : ITokenProvider
{
    SymmetricSecurityKey _key;
    private static readonly string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    IConfiguration _config;
    public TokenProvider(IConfiguration config)
    {
        _config = config;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["SigningKey"]!));
    }
    public string GenerateAccessToken(UserId userId, CustomerId customerId, Email email, string role, string jti)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, jti),
            new Claim("CustomerId", customerId.ToString()),
            new Claim(ClaimTypes.Role, role)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(10),
            SigningCredentials = credentials,
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"]

        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }

    public string GenerateRandomString(int length)
    {
        Random random = new Random();
        StringBuilder result = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            result.Append(chars[random.Next(chars.Length)]);
        }

        return result.ToString();
    }
}
