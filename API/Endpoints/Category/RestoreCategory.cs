using API.Extentions;
using API.Infrastructure;
using Application.Features.CategoryFeature.Commands.RestoreCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Category;

internal sealed class RestoreCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPatch("api/categories/{categoryId:guid}/restore", async (
            [FromRoute] Guid categoryId,
            ISender sender) =>
        {
            var result = await sender.Send(new RestoreCategoryCommand(categoryId));
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .WithTags(EndpointTag.Category)
        .WithName(EndpointName.Category.Restore)
        .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>();
    }
}
