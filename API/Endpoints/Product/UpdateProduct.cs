using API.Extentions;
using API.Infrastructure;
using Application.Features.ProductFeature.Commands.UpdateProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Product;

internal sealed class UpdateProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/products/{productId:guid}", async (
            [FromRoute] Guid productId,
            [FromForm] string productName,
            [FromForm] IFormFile? productImage,
            [FromForm] string description,
            [FromForm] decimal productPrice,
            [FromForm] Guid categoryId,
            ISender sender) =>
        {
            var result = await sender.Send(new UpdateProductCommand(
                productId, 
                productName, 
                productImage, 
                description, 
                productPrice, 
                categoryId));

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .WithTags(EndpointTag.Product)
        .WithName(EndpointName.Product.Update)
        .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>();
    }
}
