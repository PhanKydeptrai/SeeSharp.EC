using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Commands.RevokeAllCustomerRefreshTokens;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;
using System.IdentityModel.Tokens.Jwt;

namespace API.Endpoints.Customer;

internal sealed class RevokeAllCustomerRefreshTokens : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapDelete("api/customers/{userId:guid}/refresh-tokens", async (
            [FromRoute] Guid userId,
            ISender sender,
            HttpContext httpContext) =>
        {
            string token = TokenExtentions.GetTokenFromHeader(httpContext)!;
            var claims = TokenExtentions.DecodeJwt(token);
            claims.TryGetValue(JwtRegisteredClaimNames.Sub, out var sub);
            if (sub != userId.ToString())
            {
                return Results.Unauthorized();
            }

            var result = await sender.Send(new RevokeAllCustomerRefreshTokensCommand(userId));
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .WithTags(EndpointTags.Customer)
        .WithName(EndpointName.Customer.RevokeRefreshTokens)
        .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>()
        .RequireAuthorization();
    }
}
