
using API.Extentions;
using API.Infrastructure;
using Application.Features.CategoryFeature.Commands.DeleteCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Category;

internal sealed class DeleteCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/categories/{categoryId}", 
        async (
            [FromRoute] string categoryId,
            HttpContext context,
            ISender sender) =>
        {
            var command = new DeleteCategoryCommand(categoryId);
            var result = await sender.Send(command);
            return result.Match(Results.NoContent, CustomResults.Problem);
        }); 
    }
}
