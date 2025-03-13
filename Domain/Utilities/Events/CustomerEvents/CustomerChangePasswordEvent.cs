namespace Domain.Utilities.Events.CustomerEvents;

public record CustomerChangePasswordEvent(Guid UserId ,Guid TokenId, string Email, Guid MessageId);
