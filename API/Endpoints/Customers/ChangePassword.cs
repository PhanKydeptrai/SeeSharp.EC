using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Commands.CustomerChangePassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;
using System.IdentityModel.Tokens.Jwt;

namespace API.Endpoints.Customers;

internal sealed class ChangePassword : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/customers/change-password", async (
            [FromBody] ChangePasswordRequest request,
            HttpContext httpContext,
            ISender sender) =>
        {
            string token = TokenExtentions.GetTokenFromHeader(httpContext)!;
            var claims = TokenExtentions.DecodeJwt(token);
            claims.TryGetValue(JwtRegisteredClaimNames.Sub, out var sub);

            var result = await sender.Send(new CustomerChangePasswordCommand(
                Guid.Parse(sub!),
                request.currentPassword,
                request.newPassword,
                request.repeatNewPassword));

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Customer)
        .WithName(EndpointName.Customer.ChangePassword)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddNotFoundResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Thay đổi mật khẩu")
        .WithDescription("""
            Cho phép khách hàng thay đổi mật khẩu của mình.
            
            Sample Request:
            
                POST /api/customers/change-password
            
            """)
        .WithOpenApi();
    }

    private class ChangePasswordRequest
    {
        public string currentPassword { get; set; } = string.Empty;
        public string newPassword { get; set; } = string.Empty;
        public string repeatNewPassword { get; set; } = string.Empty;
    }
} 