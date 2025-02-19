using API.Extentions;
using API.Infrastructure;
using Application.Features.CategoryFeature.Queries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Category;

internal sealed class GetCategoryById : IEndpoint
{   
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/categories/{categoryId}", async (
            [FromRoute] string categoryId,
            ISender sender) =>
        {
            var result = await sender.Send(new GetCategoryByIdQuery(categoryId));
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .WithTags("Category")
        .WithName("GetCategoryById");

    }
}
