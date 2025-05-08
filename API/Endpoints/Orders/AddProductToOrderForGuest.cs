using API.Extentions;
using API.Infrastructure;
using Application.Features.OrderFeature.Commands.AddProductToOrderForGuest;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Orders;

internal sealed class AddProductToOrderForGuest : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/orders/guest", async (
            [FromBody] AddProductToOrderForGuestRequest request,
            HttpContext httpContext,
            ISender sender) =>
        {
            string? token = TokenExtentions.GetTokenFromHeader(httpContext);
            var claims = TokenExtentions.DecodeJwt(token!);
            claims.TryGetValue(CustomJwtRegisteredClaimNames.GuestId, out var guestId);

            var command = new AddProductToOrderForGuestCommand(request.ProductVariantId, Guid.Parse(guestId!), request.Quantity);
            var result = await sender.Send(command);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Order)
        .WithName(EndpointName.Order.AddProductToOrderForGuest)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Thêm sản phẩm vào giỏ hàng cho khách hàng không đăng nhập")
        .WithDescription("""
            Cho phép khách hàng không đăng nhập thêm sản phẩm vào giỏ hàng.
              
            Sample Request:
              
                POST /api/orders/guest
                {
                    "productVariantId": "b2c4f3d8-5a1e-4c6b-9f7e-0d1f2e3a4b5c",
                    "quantity": 2
                }
            """)
        .WithOpenApi();
    }

    private class AddProductToOrderForGuestRequest
    {
        /// <summary>
        /// Mã sản phẩm
        /// </summary>
        public Guid ProductVariantId { get; init; }

        /// <summary>
        /// Số lượng sản phẩm
        /// </summary>
        public int Quantity { get; init; }
    }
} 