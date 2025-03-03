using Domain.Entities.Customers;

namespace Domain.Utilities.Events.CustomerEvents;

public record CustomerSignedUpEvent(
    Guid UserId,
    Guid CustomerId,
    Guid VerificationTokenId,
    string UserName,
    string Email,
    string PasswordHash,
    Guid MessageId);


