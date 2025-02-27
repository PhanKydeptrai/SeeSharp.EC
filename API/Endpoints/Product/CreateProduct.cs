using API.Extentions;
using API.Infrastructure;
using Application.Features.ProductFeature.Commands.CreateProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Product;

internal sealed class CreateProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/products",
        async (
            [FromForm] string ProductName,
            [FromForm] IFormFile? ProductImage,
            [FromForm] string? Description,
            [FromForm] decimal Price,
            [FromForm] Guid? CategoryId,
            ISender sender) =>
        {
            var result = await sender.Send(
                new CreateProductCommand(
                    ProductName, 
                    ProductImage, 
                    Description, 
                    Price, 
                    CategoryId));

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .WithTags(EndpointTag.Product)
        .WithName(EndpointName.Product.Create);
    }


}
