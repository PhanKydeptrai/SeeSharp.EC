using API.Extentions;
using API.Infrastructure;
using Application.Features.OrderFeature.Queries.GetOrderByOrderId;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Orders;

internal sealed class GetOrderByOrderId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/orders/{orderId:guid}", async (
            [FromRoute] Guid orderId,
            ISender sender) =>
        {
            var query = new GetOrderByOrderIdQuery(orderId);
            var result = await sender.Send(query);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Order)
        .WithName(EndpointName.Order.GetById)
        .Produces(StatusCodes.Status200OK)
        .AddNotFoundResponse()
        .WithSummary("Lấy thông tin đơn hàng theo ID")
        .WithDescription("""
            Cho phép khách hàng lấy thông tin đơn hàng của mình theo ID.
                
            Sample Request:
                
             GET /api/orders/{orderId}
              
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