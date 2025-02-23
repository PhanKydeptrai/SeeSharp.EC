using Application.Abstractions.LinkService;

namespace Application.DTOs.Product;

public record ProductResponse(
    Guid ProductId,
    string ProductName,
    string? ImageUrl,
    string? Description,
    decimal Price,
    string Status,
    string CategoryName)
{
    public List<Link> links { get; set; } = new();
}