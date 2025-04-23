namespace Domain.Utilities.Events.CustomerEvents;

public record CustomerConfirmedSuccessfullyEvent(string voucherCode, string Email, Guid MessageId);
