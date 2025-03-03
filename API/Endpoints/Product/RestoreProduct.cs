using API.Extentions;
using API.Infrastructure;
using Application.Features.ProductFeature.Commands.RestoreProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Product;

internal sealed class RestoreProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("api/products/{productId:guid}/restore", async (
            [FromRoute] Guid productId,
            ISender sender) =>
        {
            var result = await sender.Send(new RestoreProductCommand(productId));
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .WithTags(EndpointTag.Product)
        .WithName(EndpointName.Product.Restore)
        .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>();
    }
}
