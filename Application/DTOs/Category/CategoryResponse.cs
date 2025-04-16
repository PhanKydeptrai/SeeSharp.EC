namespace Application.DTOs.Category;

public record CategoryResponse(
    Guid categoryId,
    string categoryName,
    string? imageUrl,
    string categoryStatus,
    bool isDefault);
