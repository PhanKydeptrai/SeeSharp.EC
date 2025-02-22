namespace Domain.Utilities.Events.CategoryEvents;

public record CategoryDeletedEvent(
    Ulid categoryId,
    Ulid messageId);
