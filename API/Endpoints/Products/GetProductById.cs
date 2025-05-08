using API.Extentions;
using API.Infrastructure;
using Application.Features.ProductFeature.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Products;

internal sealed class GetProductById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/products/{id:guid}", async (
            [FromRoute] Guid id,
            ISender sender) =>
        {
            var result = await sender.Send(new GetProductByIdQuery(id));
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Product)
        .WithName(EndpointName.Product.GetProductById)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .AddNotFoundResponse()
        .WithSummary("Lấy sản phẩm theo ID")
        .WithDescription("""
            Lấy sản phẩm theo ID.
              
            Sample Request:
              
                GET /api/products/{id}
              
            """)
        .WithOpenApi(o =>
        {
            var idParam = o.Parameters.FirstOrDefault(p => p.Name == "id");

            if (idParam is not null)
            {
                idParam.Description = "ID của sản phẩm (GUID)";
            }

            return o;
        });
    }
}