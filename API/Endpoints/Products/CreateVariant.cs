using API.Extentions;
using API.Infrastructure;
using Application.Features.ProductFeature.Commands.CreateProductVariant;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Products;

internal sealed class CreateVariant : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/products/variants", async (
            [FromForm] CreateProductVariantRequest request,
            ISender sender) =>
        {
            var command = new CreateProductVariantCommand(
                request.VariantName,
                request.VariantPrice,
                request.ColorCode,
                request.Image,
                request.Description,
                request.ProductId);
        
            var result = await sender.Send(command);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Product)
        .WithName(EndpointName.Product.CreateVariant)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Tạo biến thể sản phẩm mới")
        .WithDescription("""
            Cho phép admin tạo một biến thể sản phẩm mới.
            
            Sample Request:
            
                POST /api/products/variants
            
            """)
        .WithOpenApi()
        .RequireAuthorization();
    }

    private class CreateProductVariantRequest
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
        /// Mã màu biến thể sản phẩm
        /// </summary>
        public string ColorCode { get; set; } = string.Empty;

        /// <summary>
        /// Hình ảnh biến thể sản phẩm
        /// </summary>
        public IFormFile? Image { get; set; }

        /// <summary>
        /// Mô tả biến thể sản phẩm
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Id của sản phẩm cha
        /// </summary>
        public Guid ProductId { get; set; }
    }
} 