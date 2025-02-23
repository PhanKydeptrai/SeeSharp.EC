using Domain.Entities.Products;

namespace Domain.Utilities.Events.ProductEvents;

public record ProductCreatedEvent(
    Guid ProductId,
    string ProductName,
    string? ImageUrl,
    string? Description,
    decimal Price,
    ProductStatus ProductStatus,
    Guid CategoryId,
    Guid MessageId);
