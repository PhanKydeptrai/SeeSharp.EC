
using Application.Features.ProductFeature.Queries.GetAllProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SharedKernel.Constants;

namespace API.Endpoints.Product;

public class GetAllProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/products", 
        async (
            [FromQuery] string? filterProductStatus,
            [FromQuery] string? filterCategory,
            [FromQuery] string? searchTerm,
            [FromQuery] string? sortColumn,
            [FromQuery] string? sortOrder,
            [FromQuery] int? page,
            [FromQuery] int? pageSize,
            ISender sender) => 
        { 
            var result = await sender.Send(
                new GetAllProductQuery(
                    filterProductStatus,
                    filterCategory,
                    searchTerm,
                    sortColumn,
                    sortOrder,
                    page,
                    pageSize));

            return Results.Ok(result.Value);
        })
        .DisableAntiforgery()
        .WithTags(EndpointTag.Product)
        .WithName(EndpointName.Product.GetAll);
    }
}
