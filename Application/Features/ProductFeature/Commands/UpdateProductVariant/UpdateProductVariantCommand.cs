using Application.Abstractions.Messaging;

namespace Application.Features.ProductFeature.Commands.UpdateProductVariant;

public record UpdateProductVariantCommand(
    Guid ProductVariantId,
    string VariantName,
    decimal ProductVariantPrice,
    string ColorCode,
    string? ImageUrl,
    string Description,
    Guid ProductId,
    bool IsBaseVariant) : ICommand;

