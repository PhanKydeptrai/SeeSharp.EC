namespace Domain.Utilities.Events.CustomerEvents;

public record CustomerConfirmedEvent(string voucherCode, string Email, Guid MessageId);
