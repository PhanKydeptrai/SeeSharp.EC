namespace Domain.Entities.Orders;

public enum OrderPaymentStatus
{
    Waiting = 0, //Chờ xác nhận
    Paid = 1, //Đã thanh toán
    Unpaid = 2 //Chưa thanh toán
}

