using Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;

namespace Application.Features.ProductFeature.Commands.UpdateProduct;
public record UpdateProductCommand(
    Guid ProductId, 
    string ProductName,
    IFormFile? ProductImage,
    string? Description,
    decimal ProductPrice,
    Guid? CategoryId) : ICommand;
