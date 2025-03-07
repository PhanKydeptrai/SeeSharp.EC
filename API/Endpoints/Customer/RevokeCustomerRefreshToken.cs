
using System.IdentityModel.Tokens.Jwt;
using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Commands.CustomerRevokeRefreshToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Customer;

internal sealed class RevokeCustomerRefreshToken : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        //Khi nào đăng xuất thì gọi cái này
        builder.MapDelete("api/customers/{userId:guid}/refresh-token", async (
            [FromRoute] Guid userId,
            HttpContext httpContext,
            ISender sender) =>
        {
            string token = TokenExtentions.GetTokenFromHeader(httpContext);
            var claims = TokenExtentions.DecodeJwt(token);
            claims.TryGetValue(JwtRegisteredClaimNames.Sub, out var sub);
            claims.TryGetValue(JwtRegisteredClaimNames.Jti, out var jti);

            if (sub != userId.ToString()) 
            {
                return Results.Unauthorized();
            }

            var result = await sender.Send(new CustomerRevokeRefreshTokenCommand(jti!));
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .WithTags(EndpointTag.Customer)
        .WithName(EndpointName.Customer.RevokeRefreshToken)
        .RequireAuthorization();
    }
}
