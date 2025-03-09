namespace Domain.Utilities.Events.CustomerEvents;

public record CustomerSignedUpEvent(
    Guid UserId,
    Guid CustomerId,
    Guid VerificationTokenId,
    string UserName,
    string Email,
    string PasswordHash,
    Guid MessageId);


public record CustomerSignedUpWithGoogleAccountEvent(
    Guid UserId,
    Guid CustomerId,
    string UserName,
    string Email,
    string ImageUrl,
    Guid MessageId);