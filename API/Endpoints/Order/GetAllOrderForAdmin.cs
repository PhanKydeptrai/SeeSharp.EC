
using API.Extentions;
using API.Infrastructure;
using Application.Features.OrderFeature.Queries.GetAllOrderForAdmin;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Order;

internal sealed class GetAllOrderForAdmin : IEndpoint
{
    /// <summary>
    /// Endpoint to get all orders for the admin | Just get the orders with orderdstatus != New
    /// </summary>
    /// <param name="builder"></param>
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/api/orders", async (
            [FromQuery] string? statusFilter,
            [FromQuery] string? customerFilter,
            [FromQuery] string? searchTerm,
            [FromQuery] string? sortColumn,
            [FromQuery] string? sortOrder,
            [FromQuery] int? page,
            [FromQuery] int? pageSize,
            ISender sender) =>
        {
            var result = await sender.Send(new GetAllOrderForAdminQuery(
                statusFilter,
                customerFilter,
                searchTerm,
                sortColumn,
                sortOrder,
                page,
                pageSize));
            
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .RequireAuthorization()
        .WithTags(EndpointTag.Order)
        .WithName(EndpointName.Order.GetAllOrderForAdmin)
        .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>();
    }
}
