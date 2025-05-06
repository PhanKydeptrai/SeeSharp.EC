using API.Extentions;
using API.Infrastructure;
using Application.Features.OrderFeature.Commands.DeleteOrderDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Orders;

internal sealed class DeleteOrderDetail : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/orders/details/{orderDetailId:guid}", 
        async (
            [FromRoute] Guid orderDetailId,
            HttpContext httpContext, 
            ISender sender) =>
        {
            string token = TokenExtentions.GetTokenFromHeader(httpContext)!;
            var claims = TokenExtentions.DecodeJwt(token);
            claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);

            var result = await sender.Send(new DeleteOrderDetailCommand(orderDetailId));
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Order)
        .WithName(EndpointName.Order.DeleteOrderDetail)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddNotFoundResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Xóa sản phẩm trong giỏ hàng")
        .WithDescription("""
            Cho phép khách hàng xóa sản phẩm trong giỏ hàng.
              
            Sample Request:
              
                DELETE /api/orders/details/{orderDetailId}
            """)
        .WithOpenApi(o =>
        {
            var orderDetailIdParam = o.Parameters.FirstOrDefault(p => p.Name == "orderDetailId");

            if (orderDetailIdParam is not null)
            {
                orderDetailIdParam.Description = "ID của chi tiết đơn hàng (GUID)";
            }

            return o;
        })
        .RequireAuthorization();
    }
} 