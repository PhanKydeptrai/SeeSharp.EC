namespace Domain.Entities.OrderTransactions;

public enum TransactionStatus
{
    Pending = 0, //Chờ xác nhận 
    Completed = 1, //Đã hoàn thành
    Failed = 2, //Thất bại
}
