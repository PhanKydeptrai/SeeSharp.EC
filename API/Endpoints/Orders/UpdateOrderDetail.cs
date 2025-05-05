using API.Extentions;
using API.Infrastructure;
using Application.Features.OrderFeature.Commands.UpdateOrderDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Orders;

internal sealed class UpdateOrderDetail : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/orders/details/{orderDetailId:guid}", async (
            [FromRoute] Guid orderDetailId,
            [FromBody] UpdateOrderDetailRequest request,
            ISender sender) =>
        {
            var result = await sender.Send(new UpdateOrderDetailCommand(orderDetailId, request.Quantity));
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Order)
        .WithName(EndpointName.Order.UpdateOrderDetail)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddNotFoundResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Cập nhật thông tin chi tiết đơn hàng")
        .WithDescription("""
            Cho phép khách hàng cập nhật thông tin chi tiết đơn hàng của mình theo ID.
                 
            Sample Request:
                 
            PUT /api/orders/details/{orderDetailId}
               
            """)
        .WithOpenApi(operation =>
        {
            var orderDetailIdParam = operation.Parameters.FirstOrDefault(p => p.Name == "orderDetailId");

            if (orderDetailIdParam is not null)
            {
                orderDetailIdParam.Description = "ID của chi tiết đơn hàng (GUID)";
            }
            return operation;
        });
    }

    private class UpdateOrderDetailRequest
    {
        /// <summary>
        /// Số lượng sản phẩm trong đơn hàng
        /// </summary>
        public int Quantity { get; set; }
    }
}