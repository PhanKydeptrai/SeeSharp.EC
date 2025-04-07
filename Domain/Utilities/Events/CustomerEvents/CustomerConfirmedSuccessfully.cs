namespace Domain.Utilities.Events.CustomerEvents;

public record CustomerConfirmedSuccessfullyEvent(string Email, Guid MessageId);
