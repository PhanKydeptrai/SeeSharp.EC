using API.Extentions;
using API.Infrastructure;
using Application.Features.WishItemFeature.Commands.DeleteWishItemFromWishList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Wishlists;

internal sealed class DeleteWishItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/wishitems/{wishItemId:guid}", async (
            [FromRoute] Guid wishItemId,
            ISender sender) =>
        {
            var result = await sender.Send(new DeleteWishItemFromWishListCommand(wishItemId));
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Wishlist)
        .WithName(EndpointName.Wishlist.DeleteWishListItem)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddUnauthorizedResponse()
        .AddNotFoundResponse()
        .WithSummary("Xóa sản phẩm khỏi danh sách yêu thích")
        .WithDescription("""
            Cho phép người dùng xóa một sản phẩm khỏi danh sách yêu thích.
               
            Sample Request:
               
                DELETE /api/wishitems/{wishItemId}
               
            """)
        .WithOpenApi(o =>
        {
            var wishItemIdParam = o.Parameters.FirstOrDefault(p => p.Name == "wishItemId");

            if (wishItemIdParam is not null)
            {
                wishItemIdParam.Description = "ID của sản phẩm trong danh sách yêu thích (GUID)";
            }

            return o;
        })
        .RequireAuthorization();
    }
} 