
using API.Extentions;
using API.Infrastructure;
using Application.Features.WishItemFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Wishlist;

internal sealed class AddWishList : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("api/wishitems/{productId:guid}", async (
            [FromRoute] Guid productId,
            HttpContext httpContext,
            ISender sender) =>
        {
            string token = TokenExtentions.GetTokenFromHeader(httpContext);
            var claims = TokenExtentions.DecodeJwt(token);
            claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);

            var command = new AddWishListCommand(productId, Guid.Parse(customerId!));
            var result = await sender.Send(command);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTag.Wishlist)
        .WithName(EndpointName.Wishlist.AddWishList)
        .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>()
        .RequireAuthorization();
    }
}
