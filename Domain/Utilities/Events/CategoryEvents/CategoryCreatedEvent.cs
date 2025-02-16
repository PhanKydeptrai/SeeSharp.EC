using SharedKernel;

namespace Domain.Utilities.Events.CategoryEvents;

public sealed record CategoryCreatedEvent(
    Ulid categoryId,
    string categoryName,
    string imageUrl) : IDomainEvent;
