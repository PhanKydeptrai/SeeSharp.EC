namespace Domain.Utilities.Events.CustomerEvents;

public record CustomerResetPasswordSuccessNotificationEvent(
    Guid UserId, 
    string Email,
    string RandomPass,
    Guid MessageId);
