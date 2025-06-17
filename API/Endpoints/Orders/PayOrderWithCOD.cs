using API.Extentions;
using API.Infrastructure;
using Application.Features.OrderFeature.Commands.PayOrderWithCOD;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Orders;

public sealed class PayOrderWithCOD : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/orders/{orderId:guid}/pay-cod",
            async (
                [FromRoute] Guid orderId,
                ISender sender) =>
            {
                var result = await sender.Send(new PayOrderWithCODCommand(orderId));
                return result.Match(Results.Ok, CustomResults.Problem);

            }).WithTags(EndpointTags.Order)
            .WithName(EndpointName.Order.PayOrderWithCOD)
            .Produces(StatusCodes.Status200OK)
            .AddBadRequestResponse()
            .AddForbiddenResponse()
            .AddUnauthorizedResponse()
            .AddNotFoundResponse()
            .WithSummary("Thanh toán đơn hàng bằng COD (Cash On Delivery)")
            .WithDescription("""
                Cho phép khách hàng thanh toán đơn hàng bằng phương thức COD (Cash On Delivery).

                    Sample Request:

                        POST /api/orders/{orderId}/pay-cod
                """)
            .WithOpenApi(o =>
            {
                var orderIdParam = o.Parameters.FirstOrDefault(p => p.Name == "orderId");
                if (orderIdParam != null)
                {
                    orderIdParam.Description = "ID của đơn hàng cần thanh toán COD";
                }
                return o;
            });
    }
}
