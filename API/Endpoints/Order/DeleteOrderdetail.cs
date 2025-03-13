using API.Extentions;
using API.Infrastructure;
using Application.Features.OrderFeature.Commands.DeleteOrderDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Order;

internal sealed class DeleteOrderdetail : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapDelete("api/orders/details/{id:guid}", async (
            [FromRoute] Guid id,
            ISender sender) =>
        {
            var result = await sender.Send(new DeleteOrderDetailCommand(id));
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>()
        .WithTags(EndpointTag.Order)
        .WithName(EndpointName.Order.DeleteOrderDetail)
        .RequireAuthorization();
    }
}
