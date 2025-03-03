namespace Domain.Utilities.Events.ProductEvents;

public record ProductRestoredEvent(Guid ProductId, Guid MessageId);
