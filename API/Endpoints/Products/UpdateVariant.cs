using API.Extentions;
using API.Infrastructure;
using Application.Features.ProductFeature.Commands.UpdateProductVariant;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Products;

internal sealed class UpdateVariant : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/products/variants/{productVariantId:guid}", async (
            [FromRoute] Guid productVariantId,
            [FromForm] UpdateProductVariantRequest request,
            ISender sender) =>
        {
            var command = new UpdateProductVariantCommand(
                productVariantId,
                request.VariantName,
                request.VariantPrice,
                request.ColorCode,
                request.Image,
                request.Description,
                request.ProductId,
                request.IsBaseVariant);

            var result = await sender.Send(command);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Product)
        .WithName(EndpointName.Product.UpdateVariant)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddUnauthorizedResponse()
        .AddNotFoundResponse()
        .WithSummary("Cập nhật thông tin biến thể sản phẩm")
        .WithDescription("""
            Cho phép admin cập nhật thông tin biến thể sản phẩm.
               
            Sample Request:
               
                PUT /api/products/variants/{productVariantId}
               
            """)
        .WithOpenApi(o =>
        {
            var productVariantIdParam = o.Parameters.FirstOrDefault(p => p.Name == "productVariantId");

            if (productVariantIdParam is not null)
            {
                productVariantIdParam.Description = "ID của biến thể sản phẩm (GUID)";
            }

            return o;
        })
        .RequireAuthorization();
    }

    private class UpdateProductVariantRequest
    {
        /// <summary>
        /// Tên biến thể sản phẩm
        /// </summary>
        public string VariantName { get; set; } = string.Empty;

        /// <summary>
        /// Giá biến thể sản phẩm
        /// </summary>
        public decimal VariantPrice { get; set; }

        /// <summary>
        /// Mã màu của biến thể sản phẩm
        /// </summary>
        public string ColorCode { get; set; } = string.Empty;

        /// <summary>
        /// Hình ảnh của biến thể sản phẩm
        /// </summary>
        public IFormFile? Image { get; set; }

        /// <summary>
        /// Mô tả của biến thể sản phẩm
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// ID của sản phẩm
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Có phải là biến thể chính không
        /// </summary>
        public bool IsBaseVariant { get; set; } = false;
    }
}