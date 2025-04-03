using Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;

namespace Application.Features.ProductFeature.Commands.UpdateProductVariant;

public record UpdateProductVariantCommand(
    Guid ProductVariantId,
    string VariantName,
    decimal ProductVariantPrice,
    string ColorCode,
    IFormFile? Image,
    string Description,
    Guid ProductId,
    bool IsBaseVariant) : ICommand;

