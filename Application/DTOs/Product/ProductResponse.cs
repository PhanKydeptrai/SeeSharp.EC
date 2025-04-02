using Application.Abstractions.LinkService;

namespace Application.DTOs.Product;

public record ProductResponse(
    Guid ProductId,
    string ProductName,
    string? ImageUrl,
    string? Description,
    string Status,
    string CategoryName,
    VariantResponse[] Variants)
{
    public List<Link> links { get; set; } = new();
}

public record VariantResponse(
    Guid ProductVariantId,
    string VariantName,
    string ColorCode,
    string Description,
    decimal Price,
    string ImageUrl,
    bool IsBaseVariant);