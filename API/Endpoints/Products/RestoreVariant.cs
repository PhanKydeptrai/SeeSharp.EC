using API.Extentions;
using API.Infrastructure;
using Application.Features.ProductFeature.Commands.RestoreVariant;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Products;

internal sealed class RestoreVariant : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("api/products/variants/{variantId:guid}/restore", async (
            [FromRoute] Guid variantId,
            ISender sender) =>
        {
            var command = new RestoreVariantCommand(variantId);
            var result = await sender.Send(command);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Product)
        .WithName(EndpointName.Product.RestoreVariant)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddUnauthorizedResponse()
        .AddNotFoundResponse()
        .WithSummary("Khôi phục biến thể sản phẩm")
        .WithDescription("""
            Cho phép admin khôi phục một biến thể sản phẩm đã xóa.
               
            Sample Request:
               
                PATCH /api/products/variants/{variantId}/restore
               
            """)
        .WithOpenApi(o =>
        {
            var variantIdParam = o.Parameters.FirstOrDefault(p => p.Name == "variantId");

            if (variantIdParam is not null)
            {
                variantIdParam.Description = "ID của biến thể sản phẩm (GUID)";
            }

            return o;
        })
        .RequireAuthorization();
    }
}