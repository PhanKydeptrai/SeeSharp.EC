namespace Domain.Utilities.Events.CustomerEvents;

public record AccountVerificationEmailSentEvent(
    Guid UserId, 
    Guid VerificationTokenId, 
    string Email, 
    Guid MessageId);
