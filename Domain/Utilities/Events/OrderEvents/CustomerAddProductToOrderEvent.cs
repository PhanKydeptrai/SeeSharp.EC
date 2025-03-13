namespace Domain.Utilities.Events.OrderEvents;

//* Order and order detail are exist
public record CustomerAddProductToOrderEvent(
    Guid OrderId,
    Guid OrderDetailId,
    Guid CustomerId,
    Guid ProductId,
    int OrderDetailQuantity,
    decimal OrderDetailUnitPrice,
    decimal OrderTotal,
    Guid MessageId,
    OrderMessageType MessageType);

public enum OrderMessageType
{
    CreateOrderDetailAndUpdateOrder = 0,
    UpdateOrderDetailAndUpdateOrder = 1,
    CreateAll = 2,
}