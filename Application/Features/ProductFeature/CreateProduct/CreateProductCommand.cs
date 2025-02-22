using Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;

namespace Application.Features.ProductFeature.CreateProduct;

public record CreateProductCommand(
    string ProductName,
    IFormFile? ProductImage,
    string? Description,
    decimal Price,
    string CategoryId) : ICommand;
