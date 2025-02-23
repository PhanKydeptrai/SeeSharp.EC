using API.Extentions;
using API.Infrastructure;
using Application.Features.CategoryFeature.Commands.CreateCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Category;
internal sealed class CreateCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/categories", 
        async (
            [FromForm] string categoryName,
            [FromForm] IFormFile? image,
            HttpContext context,
            ISender sender) =>
        {
            var command = new CreateCategoryCommand(categoryName, image);
            var result = await sender.Send(command);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .WithTags(EndpointTag.Category)
        .WithName(EndpointName.Category.Create);
    }
}
