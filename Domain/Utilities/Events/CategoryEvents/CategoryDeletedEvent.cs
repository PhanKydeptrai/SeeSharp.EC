namespace Domain.Utilities.Events.CategoryEvents;

public record CategoryDeletedEvent(
    Guid categoryId,
    Guid messageId);
