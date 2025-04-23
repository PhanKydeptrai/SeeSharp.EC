namespace Domain.Entities.OrderTransactions;

public enum TransactionStatus
{
    Pending = 0, //Chờ xác nhận 
    Processing = 1, //Đang xử lý
    Completed = 2, //Đã hoàn thành
    Failed = 3, //Thất bại
}
