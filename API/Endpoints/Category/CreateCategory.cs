using API.Extentions;
using API.Infrastructure;
using Application.Features.CategoryFeature;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Category;
internal sealed class CreateCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/categories", 
        async (
            [FromForm] string categoryName,
            [FromForm] IFormFile? image,
            ISender sender) =>
        {
            var command = new CreateCategoryCommand(categoryName, image);
            var result = await sender.Send(command);
            return result.Match(Results.Created, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .WithTags("Category");
    }
}
