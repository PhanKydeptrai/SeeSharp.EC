using Domain.Entities.Products;
using SharedKernel;

namespace Domain.Utilities.Events.ProductEvents;

public record ProductUpdatedEvent(
    Guid ProductId,
    string ProductName,
    string? ImageUrl,
    string? Description,
    decimal Price,
    ProductStatus ProductStatus,
    Guid? CategoryId,
    Guid MessageId
    ) : IDomainEvent;
