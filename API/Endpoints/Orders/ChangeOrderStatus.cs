using API.Extentions;
using API.Infrastructure;
using Application.Features.OrderFeature.Commands.ChangeOrderStatus;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Orders;

internal sealed class ChangeOrderStatus : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("api/orders/{orderId:guid}/order-status", 
        async (
            [FromRoute] Guid orderId,
            ISender sender) =>
        {
            var result = await sender.Send(new ChangeOrderStatusCommand(orderId));
            return result.Match(Results.NoContent, CustomResults.Problem);

        }).WithTags(EndpointTags.Order)
        .WithName(EndpointName.Order.ChangeOrderStatus)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddUnauthorizedResponse()
        .AddNotFoundResponse()
        .WithSummary("Thay đổi trạng thái đơn hàng")
        .WithDescription("""
            Cho phép nhân viên thay đổi trạng thái đơn hàng.
              
            Sample Request:
              
                PATCH /api/orders/{orderId}/order-status
              
            """)
        .WithOpenApi(o =>
        {
            var orderIdParam = o.Parameters.FirstOrDefault(p => p.Name == "orderId");

            if (orderIdParam is not null)
            {
                orderIdParam.Description = "ID của đơn hàng (GUID)";
            }

            return o;
        })
        .RequireAuthorization();
    }
} 