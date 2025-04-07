
using API.Extentions;
using API.Infrastructure;
using Application.Features.OrderFeature.Queries.GetOrderByOrderId;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Order;

internal sealed class GetOrderByOrderId : IEndpoint
{
    /// <summary>
    /// Use this endpoint to get order details by order id. 
    /// Usecase 1: Admin want to see the details of an order. | Requires Admin Role and Order Status is not "New"
    /// </summary>
    /// <param name="builder"></param>
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/api/orders/{orderId:guid}", async (
            [FromRoute] Guid orderId,
            ISender sender) => 
        {
            var result = await sender.Send(new GetOrderByOrderIdQuery(orderId));
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .WithTags(EndpointTags.Order)
        .WithName(EndpointName.Order.GetOrderByOrderId)
        .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>()
        .RequireAuthorization();
    }
}
