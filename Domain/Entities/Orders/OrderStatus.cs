namespace Domain.Entities.Orders;

public enum OrderStatus
{
    Waiting = 0, //Chờ xác nhận | Admin chưa xem được.
    New = 1, //Mới | 👇 Admin đã xem được.
    Processing = 2, //Đang xử lý
    Shipped = 3, //Đã giao hàng
    Delivered = 4, //Đã nhận hàng
    Cancelled = 5 //Đã hủy
}

