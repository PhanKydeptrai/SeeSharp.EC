namespace Domain.Utilities.Events.OrderEvents;

public record CustomerUpdateOrderDetailEvent(
    Guid OrderDetailId,
    int OrderDetailQuantity,
    decimal OrderDetailUnitPrice,
    decimal OrderTotal,
    Guid MessageId);

