using Domain.Entities.Products;

namespace Domain.Utilities.Events.ProductEvents;

public record ProductCreatedEvent(
    Ulid ProductId,
    string ProductName,
    string? ImageUrl,
    string? Description,
    decimal Price,
    ProductStatus ProductStatus,
    Ulid CategoryId,
    Ulid MessageId);
