namespace Domain.Utilities.Events.CustomerEvents;

public record CustomerChangePasswordSuccessNotificationEvent(string email, Guid MessageId);