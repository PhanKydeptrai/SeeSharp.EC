using API.Extentions;
using API.Infrastructure;
using Application.Features.OrderFeature.Commands.CancelOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Orders;

internal sealed class OrderCancel : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/orders/{orderId:guid}/cancel", async (
            [FromRoute] Guid orderId,
            ISender sender) =>
        {
            var result = await sender.Send(new CancelOrderCommand(orderId));
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Order)
        .WithName(EndpointName.Order.CancelOrder)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddNotFoundResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Hủy đơn hàng")
        .WithDescription("""
            Cho phép khách hàng hủy đơn hàng của mình theo ID.
                 
            Sample Request:
                 
            DELETE /api/orders/{orderId}/cancel
               
            """)
        .WithOpenApi(operation =>
        {
            var orderIdParam = operation.Parameters.FirstOrDefault(p => p.Name == "orderId");

            if (orderIdParam is not null)
            {
                orderIdParam.Description = "ID của đơn hàng (GUID)";    
            }
            return operation;
        })
        .RequireAuthorization();
    }
} 