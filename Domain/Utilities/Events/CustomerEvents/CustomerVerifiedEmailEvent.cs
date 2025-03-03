namespace Domain.Utilities.Events.CustomerEvents;

public record CustomerVerifiedEmailEvent(
    Guid UserId,
    Guid MessageId);
