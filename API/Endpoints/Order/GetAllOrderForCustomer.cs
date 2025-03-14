using API.Extentions;
using API.Infrastructure;
using Application.Features.OrderFeature.Queries.GetAllOrderForCustomer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Order;

internal sealed class GetAllOrderForCustomer : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/api/orders", async (
            [FromQuery] string? statusFilter,
            [FromQuery] string? searchTerm,
            [FromQuery] string? sortColumn,
            [FromQuery] string? sortOrder,
            [FromQuery] int? page,
            [FromQuery] int? pageSize,
            HttpContext httpContext,
            ISender sender) =>
        {
            string token = TokenExtentions.GetTokenFromHeader(httpContext);
            var claims = TokenExtentions.DecodeJwt(token);
            claims.TryGetValue("CustomerId", out var customerId);

            var query = new GetAllOrderForCustomerQuery(
                new Guid(customerId!),
                statusFilter,
                searchTerm,
                sortColumn,
                sortOrder,
                page,
                pageSize);

            var result = await sender.Send(query);
            return result.Match(Results.Ok, CustomResults.Problem);
        });
    }
}
