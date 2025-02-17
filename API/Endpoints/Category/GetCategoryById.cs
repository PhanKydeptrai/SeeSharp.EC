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
        app.MapGet("api/categories/{id}", async (
            [FromRoute] string id,
            ISender sender,
            LinkGenerator linkGenerator,
            HttpContext httpContext) =>
        {
            var result = await sender.Send(new GetCategoryByIdQuery(id));
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .WithTags("Category")
        .WithName("GetCategoryById");

    }
}
