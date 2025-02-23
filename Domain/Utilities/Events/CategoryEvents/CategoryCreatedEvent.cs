using Domain.Entities.Categories;
using SharedKernel;

namespace Domain.Utilities.Events.CategoryEvents;

public sealed record CategoryCreatedEvent(
    Guid categoryId,
    string categoryName,
    string imageUrl,
    CategoryStatus categoryStatus,
    Guid messageId) : IDomainEvent;
