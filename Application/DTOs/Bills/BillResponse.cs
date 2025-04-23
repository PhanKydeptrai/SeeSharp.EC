using Application.DTOs.Order;

namespace Application.DTOs.Bills;

public record BillResponse(
    Guid CustomerId,
    string? UserName,
    string? Email,
    string? PhoneNumber,
    string? SpecificAddress,
    decimal Total,
    string PaymentStatus,
    string OrderStatus,
    string PaymentMethod,
    string? VoucherCode,
    Guid BillId,
    decimal BillTotal,
    Guid OrderId,
    OrderDetailResponse[] OrderDetails);