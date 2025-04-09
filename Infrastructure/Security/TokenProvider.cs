using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Security;
using Domain.Entities.Customers;
using Domain.Entities.Employees;
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
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SigningKey"]!));
    }
    /// <summary>
    /// Tạo access token cho khách hàng
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="customerId"></param>
    /// <param name="email"></param>
    /// <param name="role"></param>
    /// <param name="jti"></param>
    /// <returns></returns>
    public string GenerateAccessTokenForCustomer(UserId userId, CustomerId customerId, Email email, string role, string jti)
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

    /// <summary>
    /// Tạo access token cho nhân viên
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="employeeId"></param>
    /// <param name="email"></param>
    /// <param name="role"></param>
    /// <param name="jti"></param>
    /// <returns></returns>
    public string GenerateAccessTokenForEmployee(UserId userId, EmployeeId employeeId, Email email, string role, string jti)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, jti),
            new Claim("EmployeeId", employeeId.ToString()),
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

    /// <summary>
    /// Generate access token for guest user
    /// </summary>
    /// <returns></returns>
    public string GenerateAccessTokenForGuest()
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Jti, Ulid.NewUlid().ToGuid().ToString()),
            new Claim("GuestId", Ulid.NewUlid().ToGuid().ToString()),
            new Claim(ClaimTypes.Role, "Guest")
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

    /// <summary>
    /// Tạo refresh token 
    /// </summary>
    /// <returns></returns>
    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(12));
    }

    /// <summary>
    /// tạo chuỗi ngẫu nhiên với độ dài cho trước
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
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
