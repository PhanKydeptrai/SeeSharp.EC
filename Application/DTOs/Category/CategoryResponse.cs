using Application.Abstractions.LinkService;

namespace Application.DTOs.Category;

public record CategoryResponse(
    Guid categoryId,
    string categoryName,
    string? imageUrl,
    string categoryStatus)
{
    public List<Link> links { get; set; } = new();
}
