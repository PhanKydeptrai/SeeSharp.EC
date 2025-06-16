using Domain.Entities.Bills;
using Domain.Entities.Orders;
using Domain.Entities.Users;
using Domain.Entities.Vouchers;

namespace Domain.Entities.OrderTransactions;
public sealed class OrderTransaction
{
    public OrderTransactionId OrderTransactionId { get; private set; } = null!;
    public PayerName? PayerName { get; private set; } = PayerName.Empty;
    public Email? PayerEmail { get; private set; } = Email.Empty;
    public AmountOfOrderTransaction Amount { get; private set; } = null!;
    public DescriptionOfOrderTransaction Description { get; private set; } = null!; //Nội dung giao dịch
    public PaymentMethod PaymentMethod { get; private set; }
    public IsVoucherUsed IsVoucherUsed { get; private set; } = null!;
    public TransactionStatus TransactionStatus { get; private set; }
    public VoucherId? VoucherId { get; private set; } = null!;
    public OrderId OrderId { get; private set; } = null!;
    public BillId? BillId { get; private set; } = null!;

    //* Foreign Key
    public Order? Order { get; set; } = null!;
    public Voucher? Voucher { get; set; }
    public Bill? Bill { get; set; }
    private OrderTransaction(
        OrderTransactionId orderTransactionId,
        PayerName payerName,
        Email payerEmail,
        AmountOfOrderTransaction amount,
        DescriptionOfOrderTransaction description,
        PaymentMethod paymentMethod,
        IsVoucherUsed isVoucherUsed,
        TransactionStatus transactionStatus,
        VoucherId? voucherId,
        OrderId orderId,
        BillId? billId)
    {
        OrderTransactionId = orderTransactionId;
        PayerName = payerName;
        PayerEmail = payerEmail;
        Amount = amount;
        Description = description;
        PaymentMethod = paymentMethod;
        IsVoucherUsed = isVoucherUsed;
        TransactionStatus = transactionStatus;
        VoucherId = voucherId;
        OrderId = orderId;
        BillId = billId;
    }

    public static OrderTransaction NewOrderTransaction(
        PayerName? payerName,
        Email? payerEmail,
        AmountOfOrderTransaction amount,
        DescriptionOfOrderTransaction description,
        PaymentMethod paymentMethod,
        IsVoucherUsed isVoucherUsed,
        TransactionStatus transactionStatus,
        VoucherId? voucherId,
        OrderId orderId,
        BillId? billId)
    {
        return new OrderTransaction(
            OrderTransactionId.New(),
            payerName ?? PayerName.Empty,
            payerEmail ?? Email.Empty,
            amount,
            description,
            paymentMethod,
            isVoucherUsed,
            transactionStatus,
            voucherId ?? null,
            orderId,
            billId);
    }

    public void ChangeTransactionStatus(TransactionStatus transactionStatus)
    {
        TransactionStatus = transactionStatus;
    }
}
