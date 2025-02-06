using Domain.Entities.Bills;
using Domain.Entities.Orders;
using Domain.Entities.Users;
using Domain.Entities.Vouchers;

namespace Domain.Entities.OrderTransactions;
//NOTE: Create factory method
public sealed class OrderTransaction
{
    public OrderTransactionId OrderTransactionId { get; private set; } = null!;
    public PayerName? PayerName { get; private set; } = PayerName.Empty;
    public Email? PayerEmail { get; private set; } = Email.Empty;
    public AmountOfOrderTransaction Amount { get; private set; } = null!;
    public DescriptionOfOrderTransaction Description { get; private set; } = null!; //Nội dung giao dịch
    public PaymentMethod PaymentMethod { get; private set; }
    public IsVoucherUsed IsVoucherUsed { get; private set; } = null!;
    public VoucherId? VoucherId { get; private set; } = null!;
    public OrderId OrderId { get; private set; } = null!;
    public BillId? BillId { get; private set; } = null!;

    //* Foreign Key
    public Order? Order { get; set; } = null!;
    public Voucher? Voucher { get; set; }
    public Bill? Bill { get; set; }
}
