using API.Extentions;
using API.Infrastructure;
using Application.Features.ProductFeature.Commands.DeleteProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Products;

internal sealed class DeleteProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/products/{productId:guid}", async (
            [FromRoute] Guid productId,
            ISender sender) =>
        {
            var command = new DeleteProductCommand(productId);
            var result = await sender.Send(command);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Product)
        .WithName(EndpointName.Product.DeleteProduct)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddUnauthorizedResponse()
        .AddNotFoundResponse()
        .WithSummary("Xóa sản phẩm")
        .WithDescription("""
            Cho phép admin xóa một sản phẩm.
            
            Sample Request:
            
                DELETE /api/products/{productId}
            
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