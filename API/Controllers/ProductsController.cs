using API.Extentions;
using API.Infrastructure;
using Application.Features.ProductFeature.Commands.CreateProduct;
using Application.Features.ProductFeature.Commands.DeleteProduct;
using Application.Features.ProductFeature.Commands.DeleteVariant;
using Application.Features.ProductFeature.Commands.RestoreProduct;
using Application.Features.ProductFeature.Commands.UpdateProduct;
using Application.Features.ProductFeature.Queries.GetAllProduct;
using Application.Features.ProductFeature.Queries.GetAllVariantQuery;
using Application.Features.ProductFeature.Queries.GetProductById;
using Application.Features.ProductFeature.Queries.GetVariantById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Controllers;

[Route("api/products")]
[ApiController]
[ApiKey]
public sealed class ProductsController : ControllerBase
{
    private readonly ISender _sender;
    
    public ProductsController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Get all products
    /// </summary>
    /// <param name="filterProductStatus"></param>
    /// <param name="filterCategory"></param>
    /// <param name="searchTerm"></param>
    /// <param name="sortColumn"></param>
    /// <param name="sortOrder"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    [EndpointName(EndpointName.Product.GetAll)]
    public async Task<IResult> GetAllProduct(
        [FromQuery] string? filterProductStatus,
        [FromQuery] string? filterCategory,
        [FromQuery] string? searchTerm,
        [FromQuery] string? sortColumn,
        [FromQuery] string? sortOrder,
        [FromQuery] int? page,
        [FromQuery] int? pageSize)
    {
        var result = await _sender.Send(
            new GetAllProductQuery(
                filterProductStatus,
                filterCategory,
                searchTerm,
                sortColumn,
                sortOrder,
                page,
                pageSize));

        return Results.Ok(result.Value);
    }
    
    /// <summary>
    /// Get all product variants
    /// </summary>
    /// <param name="filterProductStatus"></param>
    /// /// <param name="filterProduct"></param>
    /// <param name="filterCategory"></param>
    /// <param name="searchTerm"></param>
    /// <param name="sortColumn"></param>
    /// <param name="sortOrder"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet("variants")]
    [EndpointName(EndpointName.Product.GetAllVariant)]
    public async Task<IResult> GetGetAllVariant(
        [FromQuery] string? filterProductStatus,
        [FromQuery] string? filterProduct,
        [FromQuery] string? filterCategory,
        [FromQuery] string? searchTerm,
        [FromQuery] string? sortColumn,
        [FromQuery] string? sortOrder,
        [FromQuery] int? page,
        [FromQuery] int? pageSize)
    {
        var result = await _sender.Send(
            new GetAllVariantQuery(
                filterProductStatus,
                filterProduct,
                filterCategory,
                searchTerm,
                sortColumn,
                sortOrder,
                page,
                pageSize));

        return Results.Ok(result.Value);
    }


    /// <summary>
    /// Get a product by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [EndpointName(EndpointName.Product.GetById)]
    public async Task<IResult> GetProductById([FromRoute] Guid id)
    {
        var result = await _sender.Send(new GetProductByIdQuery(id));
        return result.Match(Results.Ok, CustomResults.Problem);
    }

    /// <summary>
    /// Get a product variant by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("variants/{id:guid}")]
    [EndpointName(EndpointName.Product.GetVariantById)]
    public async Task<IResult> GetVariantById([FromRoute] Guid id)
    {
        var result = await _sender.Send(new GetVariantByIdQuery(id));
        return result.Match(Results.Ok, CustomResults.Problem);
    }

    /// <summary>
    /// Tạo sản phẩm mới
    /// </summary>
    /// <param name="ProductName">Tên sản phẩm</param>
    /// <param name="ProductBaseVariantName">Tên phiên bản gốc</param>
    /// <param name="ColorCode">Mã màu của phiên bản gốc</param>
    /// <param name="ProductImage">Ảnh sản phẩm, ảnh này cũng được sử dụng cho sản phẩm gốc</param>
    /// <param name="Description">Mô tả sản phẩm</param>
    /// <param name="VariantPrice">Giá sản phẩm, nó được áp dụng cho sản phẩm gốc</param>
    /// <param name="CategoryId">Mã danh mục</param>
    /// <returns></returns>
    [HttpPost]
    [EndpointName(EndpointName.Product.Create)]
    public async Task<IResult> CreateProduct(
        [FromForm] string ProductName,
        [FromForm] string ProductBaseVariantName,
        [FromForm] string ColorCode,
        IFormFile? ProductImage,
        [FromForm] string? Description,
        [FromForm] decimal VariantPrice,
        [FromForm] Guid? CategoryId)
    {
        var command = new CreateProductCommand(
            ProductName,
            ProductImage,
            ProductBaseVariantName,
            ColorCode,
            Description, 
            VariantPrice, 
            CategoryId);
            
        var result = await _sender.Send(command);
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Update a product
    /// </summary>
    /// <returns></returns>
    [HttpPut("{productId:guid}")]
    [EndpointName(EndpointName.Product.Update)]
    public async Task<IResult> UpdateProduct(
        [FromRoute] Guid productId,
        [FromForm] string productName,
        [FromForm] IFormFile? productImage,
        [FromForm] string description,
        [FromForm] decimal productPrice,
        [FromForm] Guid categoryId)
    {
        var command = new UpdateProductCommand(
            productId, 
            productName, 
            productImage, 
            description, 
            productPrice, 
            categoryId);
        var result = await _sender.Send(command);
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Delete a product
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    [HttpDelete("{productId:guid}")]
    [EndpointName(EndpointName.Product.Delete)]
    public async Task<IResult> DeleteProduct([FromRoute] Guid productId)
    {
        var command = new DeleteProductCommand(productId);
        var result = await _sender.Send(command);
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Restore a product
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    [HttpPatch("{productId:guid}/restore")]
    [EndpointName(EndpointName.Product.Restore)]
    public async Task<IResult> RestoreProduct([FromRoute] Guid productId)
    {
        var result = await _sender.Send(new RestoreProductCommand(productId));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Delete a product variant
    /// </summary>
    /// <param name="variantId"></param>
    /// <returns></returns>
    [HttpDelete("variants/{variantId:guid}")]
    [EndpointName(EndpointName.Product.DeleteVariant)]
    public async Task<IResult> DeleteVariant([FromRoute] Guid variantId)
    {
        var command = new DeleteVariantCommand(variantId);
        var result = await _sender.Send(command);
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Restore a product variant
    /// </summary>
    /// <param name="variantId"></param>
    /// <returns></returns>
    [HttpPatch("variants/{variantId:guid}/restore")]
    [EndpointName(EndpointName.Product.RestoreVariant)]
    public async Task<IResult> RestoreVariant([FromRoute] Guid variantId)
    {
        var command = new DeleteVariantCommand(variantId);
        var result = await _sender.Send(command);
        return result.Match(Results.NoContent, CustomResults.Problem);
    }
} 