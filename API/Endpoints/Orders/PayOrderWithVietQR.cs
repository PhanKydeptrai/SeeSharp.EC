
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Constants;

namespace API.Endpoints.Orders;

public sealed class PayOrderWithVietQR : IEndpoint
{
    private const string VALID_USERNAME = "";
    private const string VALID_PASSWORD = ""; // Base64 của username:password
    private const string SECRET_KEY = ""; // Bí mật để ký JWT token

    public void MapEndpoint(IEndpointRouteBuilder app)
    {

        app.MapPost("vqr/api/token_generate", (
            [FromHeader] string Authorization) =>
        {
            // Kiểm tra Authorization header
            if (string.IsNullOrEmpty(Authorization) || !Authorization.StartsWith("Basic "))
            {
                return Results.BadRequest("Authorization header is missing or invalid");
            }

            // Giải mã Base64
            var base64Credentials = Authorization.Substring("Basic ".Length).Trim();
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(base64Credentials));
            var values = credentials.Split(':', 2);

            if (values.Length != 2)
            {
                return Results.BadRequest("Invalid Authorization header format");
            }

            var username = values[0];
            var password = values[1];

            // Kiểm tra username và password
            if (username == VALID_USERNAME && password == VALID_PASSWORD)
            {
                var token = GenerateJwtToken(username);
                return Results.Ok(new
                {
                    access_token = token,
                    token_type = "Bearer",
                    expires_in = 300 // Thời gian hết hạn token
                });
            }
            else
            {
                return Results.Unauthorized();
            }
        })
        .WithTags(EndpointTags.Order);


    }

    // Hàm tạo JWT token
    private string GenerateJwtToken(string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(SECRET_KEY);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Name, username)
            }),
            Expires = DateTime.UtcNow.AddMinutes(5), // Token hết hạn sau 5 phút
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
