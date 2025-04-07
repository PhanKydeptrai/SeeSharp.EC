namespace Domain.Utilities.Events.CustomerEvents;

public record CustomerResetPasswordEmailSendEvent(
    Guid UserId, 
    Guid VerificationTokenId, 
    string Email, 
    Guid MessageId);
