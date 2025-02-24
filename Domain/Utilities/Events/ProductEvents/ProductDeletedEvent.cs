using SharedKernel;

namespace Domain.Utilities.Events.ProductEvents;

public record ProductDeletedEvent(Guid ProductId, Guid MessageId) : IDomainEvent;
