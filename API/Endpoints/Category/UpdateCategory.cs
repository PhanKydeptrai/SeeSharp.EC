using API.Extentions;
using API.Infrastructure;
using Application.Features.CategoryFeature.Commands.UpdateCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Category;

internal sealed class UpdateCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/categories/{categoryId}", async (
            [FromRoute] string categoryId,
            [FromForm] string categoryName,
            [FromForm] IFormFile? image,
            HttpContext context,
            ISender sender) =>
        {
            var result = await sender.Send(new UpdateCategoryCommand(categoryId, categoryName, image));
            return result.Match(
                Results.NoContent, 
                CustomResults.Problem);
        })
        .DisableAntiforgery()
        .WithTags("Category")
        .WithName("UpdateCategory");
    }
}