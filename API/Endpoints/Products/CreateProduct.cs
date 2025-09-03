using API.Extentions;
using API.Infrastructure;
using Application.Features.ProductFeature.Commands.CreateProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Products;

internal sealed class CreateProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/products", async (
            [FromForm] CreateProductRequest request,
            ISender sender) =>
        {
            var command = new CreateProductCommand(
                request.ProductName,
                request.ProductImage,
                request.ProductBaseVariantName,
                request.ColorCode,
                request.Description,
                request.VariantPrice,
                request.CategoryId);

            var result = await sender.Send(command);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Product)
        .WithName(EndpointName.Product.CreateProduct)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Tạo sản phẩm mới")
        .WithDescription("""
            Cho phép admin tạo một sản phẩm mới.
            
            Sample Request:
            
                POST /api/products
            
            """)
        .WithOpenApi()
        .RequireAuthorization()
        .DisableAntiforgery();
    }

    private class CreateProductRequest
    {
        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Tên biến thể sản phẩm
        /// </summary>
        public string ProductBaseVariantName { get; set; } = string.Empty;

        /// <summary>
        /// Mã màu sản phẩm
        /// </summary>
        public string ColorCode { get; set; } = string.Empty;
        
        /// <summary>
        /// Hình ảnh sản phẩm
        /// </summary>
        public IFormFile? ProductImage { get; set; }

        /// <summary>
        /// Mô tả sản phẩm
        /// </summary>
        public string? Description { get; set; } = string.Empty;
        
        /// <summary>
        /// Giá sản phẩm
        /// </summary>
        public decimal VariantPrice { get; set; }
        
        /// <summary>
        /// ID danh mục sản phẩm
        /// </summary>
        public Guid? CategoryId { get; set; }
    }
} 