using API.Extentions;
using API.Infrastructure;
using Application.Features.ProductFeature.Commands.DeleteProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Product;

internal sealed class DeleteProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/products/{productId:guid}", async (
            [FromRoute] Guid productId,
            ISender sender) =>
        {
            var result = await sender.Send(new DeleteProductCommand(productId));
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .WithTags(EndpointTags.Product)
        .WithName(EndpointName.Product.Delete)
        .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>();
    }
}