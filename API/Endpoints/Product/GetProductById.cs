using API.Extentions;
using API.Infrastructure;
using Application.Features.ProductFeature.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Product;

public class GetProductById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/products/{productId:guid}",
        async (
            [FromRoute] Guid productid,
            ISender sender) =>
        {
            var result = await sender.Send(new GetProductByIdQuery(productid));
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .WithTags(EndpointTag.Product)
        .WithName(EndpointName.Product.GetById);
    }
}
