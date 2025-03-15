namespace Application.DTOs.WishItems;

public record WishlistResponse(
    Guid WishItemId,
    Guid ProductId,
    string ProductName,
    string? ImageUrl,
    string? Description,
    decimal Price,
    string Status,
    string CategoryName);
