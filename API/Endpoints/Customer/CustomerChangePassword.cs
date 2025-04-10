
using System.IdentityModel.Tokens.Jwt;
using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Commands.CustomerChangePassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Customer;

internal sealed class CustomerChangePassword : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPut("api/customer/change-password", async (
            [FromBody] CustomerChangePasswordRequest request,
            HttpContext httpContext,
            ISender sender) =>
        {
            string token = TokenExtentions.GetTokenFromHeader(httpContext)!;
            var claims = TokenExtentions.DecodeJwt(token);

            claims.TryGetValue(JwtRegisteredClaimNames.Sub, out var sub);

            var command = new CustomerChangePasswordCommand(
                new Guid(sub!), 
                request.currentPassword, 
                request.newPassword, 
                request.repeatNewPassword);

            var result = await sender.Send(command);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .RequireAuthorization()
        .WithTags(EndpointTags.Customer)
        .WithName(EndpointName.Customer.ChangePassword)
        .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>();
    }
}
