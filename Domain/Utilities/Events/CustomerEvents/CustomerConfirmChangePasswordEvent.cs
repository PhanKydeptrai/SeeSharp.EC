namespace Domain.Utilities.Events.CustomerEvents;

public record CustomerConfirmChangePasswordEvent(string Email, Guid MessageId);
