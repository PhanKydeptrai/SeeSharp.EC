using API.Extentions;
using API.Infrastructure;
using Application.Features.OrderFeature.Commands.UpdateOrderDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Order;

internal sealed class UpdateOrderDetail : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPut("api/orders/details/{id:guid}", async (
            [FromRoute] Guid id,
            [FromBody] UpdateOrderDetailRequest request,
            ISender sender) =>
        {
            
        })
        .WithTags(EndpointTags.Order)
        .WithName(EndpointName.Order.UpdateOrderDetail)
        .RequireAuthorization()
        .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>();
    }
}
