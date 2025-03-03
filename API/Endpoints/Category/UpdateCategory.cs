using API.Extentions;
using API.Infrastructure;
using Application.Features.CategoryFeature.Commands.UpdateCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Category;

internal sealed class UpdateCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/categories/{categoryId:guid}", async (
            [FromRoute] Guid categoryId,
            [FromForm] string categoryName,
            [FromForm] IFormFile? image,
            ISender sender) =>
        {
            var result = await sender.Send(new UpdateCategoryCommand(categoryId, categoryName, image));
            return result.Match(
                Results.NoContent, 
                CustomResults.Problem);
        })
        .DisableAntiforgery()
        .WithTags(EndpointTag.Category)
        .WithName(EndpointName.Category.Update)
        .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>();
    }
}