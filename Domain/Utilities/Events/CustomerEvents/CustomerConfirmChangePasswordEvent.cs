namespace Domain.Utilities.Events.CustomerEvents;

public record CustomerConfirmChangePasswordEvent(Guid UserId, string Email, string NewPassword, Guid MessageId);
