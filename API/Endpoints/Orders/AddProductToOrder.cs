using API.Extentions;
using API.Infrastructure;
using Application.Features.OrderFeature.Commands.AddProductToOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Orders;

internal sealed class AddProductToOrder : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/orders", async (
            [FromBody] AddProductToOrderRequest request,
            HttpContext httpContext,
            ISender sender) =>
        {
            string token = TokenExtentions.GetTokenFromHeader(httpContext)!;
            var claims = TokenExtentions.DecodeJwt(token);
            claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);
            if (customerId is null) return Results.Unauthorized();
            
            var command = new AddProductToOrderCommand(request.ProductVariantId, new Guid(customerId!), request.Quantity);
            var result = await sender.Send(command);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Order)
        .WithName(EndpointName.Order.AddProductToOrder)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddUnauthorizedResponse()
        .AddForbiddenResponse()
        .WithSummary("Thêm sản phẩm vào giỏ hàng")
        .WithDescription("""
            Cho phép khách hàng thêm sản phẩm vào giỏ hàng.
             
            Sample Request:
             
                POST /api/orders
                {
                    "productVariantId": "b2c4f3d8-5a1e-4c6b-9f7e-0d1f2e3a4b5c",
                    "quantity": 2
                }
            """)
        .WithOpenApi()
        .RequireAuthorization();

    }

    private class AddProductToOrderRequest
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