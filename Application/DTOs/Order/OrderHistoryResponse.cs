namespace Application.DTOs.Order;

/// <summary>
/// Lịch sử đơn hàng
/// </summary>
/// <param name="CustomerId">Id khách hàng</param>
/// <param name="UserName">Tên khách hàng</param>
/// <param name="Email">Email khách hàng</param>
/// <param name="PhoneNumber">Số điện thoại khách hàng</param>
/// <param name="SpecificAddress">Địa chỉ cụ thể</param>
/// <param name="Total">Tổng tiền hàng</param>
/// <param name="PaymentStatus">Trạng thái thanh toán của đơn hàng</param>
/// <param name="PaymentMethod">Phương thức thanh 4toán</param>
/// <param name="VoucherCode">Mã giảm giá đã sử dụng</param>
/// <param name="BillId">Mã hoá đơn</param>
/// <param name="BillTotal">Số tiền cần thanh toán</param>
/// <param name="OrderId">Id đơn hàng</param>
/// <param name="OrderDetails">Chi tiết đơn hàng</param>
public record OrderHistoryResponse(
    Guid CustomerId,
    string? UserName,
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
    OrderDetailResponse[] OrderDetails
);

