namespace Application.DTOs.Bills;

public record BillResponse(
    Guid CustomerId,
    Guid OrderId,
    Guid BillId,
    DateTime CreatedDate,
    string FullName,
    string PhoneNumber,
    string Email,
    string SpecificAddress,
    string Province,
    string District,
    string Ward,
    decimal BillTotal,
    string BillPaymentStatus,
    string PaymentMethod,
    string IsRated,
    string? VoucherCode,
    bool IsVoucherUsed,
    BillDetailResponse[] BillDetails);

public record BillDetailResponse(
    Guid BillDetailId,
    string ProductName,
    string VariantName,
    string ColorCode,
    decimal Price,
    int Quantity,
    string ImageUrl,
    decimal Total,
    string ProductVariantDescription);
    