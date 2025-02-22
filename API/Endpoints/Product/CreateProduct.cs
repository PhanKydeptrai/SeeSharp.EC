using API.Extentions;
using API.Infrastructure;
using Application.Features.ProductFeature.CreateProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
            [FromForm] string CategoryId,
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
        .WithTags("Product")
        .WithName("CreateProduct");

    }


}
