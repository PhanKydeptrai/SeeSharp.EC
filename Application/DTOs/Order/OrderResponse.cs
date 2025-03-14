namespace Application.DTOs.Order;

public record OrderResponse(
    Guid OrderId,
    Guid CustomerId,
    decimal Total,
    string PaymentStatus,
    string OrderStatus,
    OrderDetailResponse[] OrderDetails);


public record OrderDetailResponse(
    Guid OrderDetailId,
    Guid ProductId,
    decimal Price,
    int Quantity,
    string ImageUrl,
    decimal Total);