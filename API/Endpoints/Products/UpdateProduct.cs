using API.Extentions;
using API.Infrastructure;
using Application.Features.ProductFeature.Commands.UpdateProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Products;

internal sealed class UpdateProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/products/{productId:guid}", async (
            [FromRoute] Guid productId,
            [FromForm] UpdateProductRequest request,
            ISender sender) =>
        {
            var command = new UpdateProductCommand(
                productId,
                request.ProductName,
                request.ProductImage,
                request.Description,
                request.ColorCode,
                request.ProductPrice,
                request.CategoryId);
                
            var result = await sender.Send(command);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Product)
        .WithName(EndpointName.Product.Update)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddUnauthorizedResponse()
        .AddNotFoundResponse()
        .WithSummary("Cập nhật thông tin sản phẩm")
        .WithDescription("""
            Cho phép admin cập nhật thông tin sản phẩm.
              
            Sample Request:
              
                PUT /api/products/{productId}
              
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

    private class UpdateProductRequest
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public IFormFile? ProductImage { get; set; }
        public string ColorCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public Guid CategoryId { get; set; }
    }
}