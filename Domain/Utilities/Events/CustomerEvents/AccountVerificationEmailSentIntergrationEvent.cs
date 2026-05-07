namespace Domain.Utilities.Events.CustomerEvents;

public record AccountVerificationEmailSentIntergrationEvent(
    Guid UserId, 
    Guid VerificationTokenId, 
    string Email, 
    Guid MessageId);
