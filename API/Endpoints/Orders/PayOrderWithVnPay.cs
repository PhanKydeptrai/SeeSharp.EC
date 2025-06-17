using API.Extentions;
using API.Infrastructure;
using Application.Features.OrderFeature.Commands.PayOrderWithVnPay;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Orders;

public sealed class PayOrderWithVnPay : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/orders/{orderId:guid}/pay-vnpay", 
            async (
                [FromRoute] Guid orderId,
                ISender sender) =>
            {
                var result = await sender.Send(new PayOrderWithVnPayCommand(orderId));
                if (result.IsFailure)
                {
                    return CustomResults.Problem(result);
                }
                return Results.Redirect(result.Value!);

            }).WithTags(EndpointTags.Order)
            .WithName(EndpointName.Order.PayOrderWithVnPay)
            .Produces(StatusCodes.Status200OK)
            .AddBadRequestResponse()
            .AddForbiddenResponse()
            .AddUnauthorizedResponse()
            .AddNotFoundResponse()
            .WithSummary("Thanh toán đơn hàng bằng VnPay")
            .WithDescription("""
            Cho phép khách hàng thanh toán đơn hàng bằng phương thức VnPay.

            Sample Request:

                POST /api/orders/{orderId}/pay-vnpay
            """)
            .WithOpenApi(o =>
            {
                var orderIdParam = o.Parameters.FirstOrDefault(p => p.Name == "orderId");
                if (orderIdParam != null)
                {
                    orderIdParam.Description = "ID của đơn hàng cần thanh toán bằng VnPay";
                }
                return o;
            });
    }
}
