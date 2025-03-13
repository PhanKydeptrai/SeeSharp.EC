namespace Domain.Utilities.Events.CustomerEvents;

public record CustomerResetPasswordEvent(Guid UserId,string Email, string RandomPass, Guid MessageId);

