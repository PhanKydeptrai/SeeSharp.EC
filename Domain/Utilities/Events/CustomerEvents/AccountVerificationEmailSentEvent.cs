namespace Domain.Utilities.Events.CustomerEvents;

public sealed record AccountVerificationEmailSentEvent(
    Guid UserId,
    Guid VerificationTokenId,
    string Email,
    Guid MessageId);
