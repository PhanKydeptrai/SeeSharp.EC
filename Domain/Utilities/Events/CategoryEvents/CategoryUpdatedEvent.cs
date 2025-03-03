using Domain.Entities.Categories;

namespace Domain.Utilities.Events.CategoryEvents;

public sealed record CategoryUpdatedEvent(
    Guid categoryId,
    string categoryName,
    string imageUrl,
    CategoryStatus categoryStatus,
    Guid messageId);
