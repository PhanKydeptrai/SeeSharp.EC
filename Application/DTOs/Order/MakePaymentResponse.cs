namespace Application.DTOs.Order;

public record MakePaymentResponse(
    Guid OrderId,
    string PaymentStatus,
    decimal Total,
    string? VoucherCode,
    OrderDetailResponse[] OrderDetails);
