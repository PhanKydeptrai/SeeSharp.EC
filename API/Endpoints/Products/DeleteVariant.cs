using API.Extentions;
using API.Infrastructure;
using Application.Features.ProductFeature.Commands.DeleteVariant;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Products;

internal sealed class DeleteVariant : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/products/variants/{variantId:guid}", async (
            [FromRoute] Guid variantId,
            ISender sender) =>
        {
            var command = new DeleteVariantCommand(variantId);
            var result = await sender.Send(command);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Product)
        .WithName(EndpointName.Product.DeleteVariant)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddUnauthorizedResponse()
        .AddNotFoundResponse()
        .WithSummary("Xóa biến thể sản phẩm")
        .WithDescription("""
            Cho phép admin xóa một biến thể sản phẩm.
             
            Sample Request:
             
                DELETE /api/products/variants/{variantId}
             
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