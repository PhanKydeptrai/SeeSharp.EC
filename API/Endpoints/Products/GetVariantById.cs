using API.Extentions;
using API.Infrastructure;
using Application.Features.ProductFeature.Queries.GetVariantById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Products;

internal sealed class GetVariantById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/products/variants/{id:guid}", async (
            [FromRoute] Guid id,
            ISender sender) =>
        {
            var result = await sender.Send(new GetVariantByIdQuery(id));
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Product)
        .WithName(EndpointName.Product.GetVariantById)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .AddNotFoundResponse()
        .WithSummary("Lấy biến thể sản phẩm theo ID")
        .WithDescription("""
            Lấy biến thể sản phẩm theo ID.
               
            Sample Request:
               
                GET /api/products/variants/{id}
               
            """)
        .WithOpenApi(o =>
        {
            var idParam = o.Parameters.FirstOrDefault(p => p.Name == "id");

            if (idParam is not null)
            {
                idParam.Description = "ID của biến thể sản phẩm (GUID)";
            }

            return o;
        });
    }
} 