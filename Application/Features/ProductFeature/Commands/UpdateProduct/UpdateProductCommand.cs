using Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;

namespace Application.Features.ProductFeature.Commands.UpdateProduct;
public record UpdateProductCommand(
    Guid ProductId, 
    string ProductName,
    IFormFile? ProductImage, //Áp dụng cho sản phẩm gốc
    string Description,
    string ColorCode, //Áp dụng cho sản phẩm gốc
    decimal ProductPrice, //Áp dụng cho sản phẩm gốc
    Guid? CategoryId) : ICommand;
