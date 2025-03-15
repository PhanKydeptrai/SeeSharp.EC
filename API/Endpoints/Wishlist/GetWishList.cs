
using API.Extentions;
using API.Infrastructure;
using Application.Features.WishItemFeature.Queries.GetWishList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Wishlist;

internal sealed class GetWishList : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("api/wishitems", async (
            [FromQuery] string? productStatusFilter,
            [FromQuery] string? searchTerm,
            [FromQuery] string? sortColumn,
            [FromQuery] string? sortOrder,
            [FromQuery] int? page,
            [FromQuery] int? pageSize,
            HttpContext httpContext,
            ISender sender) =>
        {
            string token = TokenExtentions.GetTokenFromHeader(httpContext);
            var claims = TokenExtentions.DecodeJwt(token);
            claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);

            var query = new GetWishListQuery(
                productStatusFilter, 
                searchTerm, sortColumn, 
                sortOrder, page, 
                pageSize, 
                Guid.Parse(customerId!));

            var result = await sender.Send(query);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .WithTags(EndpointTag.Wishlist)
        .WithName(EndpointName.Wishlist.GetWishList)
        .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>()
        .RequireAuthorization();
    }
}
