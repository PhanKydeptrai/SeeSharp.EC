namespace Domain.Utilities.Events.OrderEvents;

public record CustomerDeleteOrderDetailEvent(
    Guid OrderDetailId,
    decimal NewOrderTotal,
    Guid MessageId);
