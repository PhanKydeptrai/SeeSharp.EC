using API.Extentions;
using API.Infrastructure;
using Application.Features.ProductFeature.Commands.RestoreProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Products;

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
        .WithTags(EndpointTags.Product)
        .WithName(EndpointName.Product.Restore)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddUnauthorizedResponse()
        .AddNotFoundResponse()
        .WithSummary("Khôi phục sản phẩm đã xóa")
        .WithDescription("""
            Cho phép admin khôi phục một sản phẩm đã xóa.
              
            Sample Request:
              
                PATCH /api/products/{productId}/restore
              
            """)
        .WithOpenApi(o =>
        {
            var productIdParam = o.Parameters.FirstOrDefault(p => p.Name == "productId");

            if (productIdParam is not null)
            {
                productIdParam.Description = "ID của sản phẩm (GUID)";
            }

            return o;
        })
        .RequireAuthorization();
    }
} 