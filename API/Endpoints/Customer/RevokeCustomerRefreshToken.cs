
using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Commands.CustomerRevokeRefreshToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
            claims.TryGetValue("sub", out var sub);

            if (sub != userId.ToString()) 
            {
                return Results.Unauthorized();
            }

            var result = await sender.Send(new CustomerRevokeRefreshTokenCommand(userId));
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .RequireAuthorization();
    }
}
