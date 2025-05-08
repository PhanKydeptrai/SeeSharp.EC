using API.Extentions;
using API.Infrastructure;
using Application.Features.WishItemFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Wishlists;

internal sealed class AddWishList : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/wishitems", async (
            [FromBody] AddWishListRequest request,
            HttpContext httpContext,
            ISender sender) =>
        {
            string token = TokenExtentions.GetTokenFromHeader(httpContext)!;
            var claims = TokenExtentions.DecodeJwt(token);
            claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);

            var command = new AddWishListCommand(request.ProductVariantId, Guid.Parse(customerId!));
            var result = await sender.Send(command);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Wishlist)
        .WithName(EndpointName.Wishlist.AddWishList)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddUnauthorizedResponse()
        .AddNotFoundResponse()
        .WithSummary("Thêm sản phẩm vào danh sách yêu thích")
        .WithDescription("""
            Cho phép người dùng thêm một sản phẩm vào danh sách yêu thích.
               
            Sample Request:
               
                POST /api/wishitems
                {
                    "productVariantId": "b0c4f2a1-3d5e-4c8b-9f6e-7a1d2e3f4b5c"
                }
               
            """)
        .WithOpenApi()
        .RequireAuthorization();
    }

    private class AddWishListRequest
    {
        /// <summary>
        /// ID của biến thể sản phẩm
        /// </summary>
        public Guid ProductVariantId { get; set; }
    }
} 