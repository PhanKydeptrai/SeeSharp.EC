namespace Application.DTOs.Category;

public record CategoryResponse(
    Ulid categoryId, 
    string categoryName, 
    string? imageUrl, 
    string categoryStatus);
