using Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;

namespace Application.Features.ProductFeature.Commands.CreateProductVariant;

public record CreateProductVariantCommand(
    string VariantName,
    decimal ProductVariantPrice,
    string ColorCode,
    IFormFile? Image,
    string ProductVariantDescription,
    Guid ProductId
) : ICommand;
