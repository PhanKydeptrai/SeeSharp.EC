namespace Domain.Entities.Bills;

public enum BillPaymentStatus
{
    Pending = 0, //Chờ xác nhận 
    Waiting = 1, //Chờ thanh toán
    Paid = 2, //Đã thanh toán
    Unpaid = 3 //Chưa thanh toán (Có thể dùng cho COD)
}