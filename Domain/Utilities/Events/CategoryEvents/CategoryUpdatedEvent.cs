using SharedKernel;

namespace Domain.Utilities.Events.CategoryEvents;

public sealed record CategoryUpdatedEvent(
    Ulid categoryId,
    string categoryName,
    string imageUrl,
    Ulid messageId) : IDomainEvent;
