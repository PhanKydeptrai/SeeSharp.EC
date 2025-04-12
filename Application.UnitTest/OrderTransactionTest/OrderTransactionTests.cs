using Domain.Entities.Bills;
using Domain.Entities.Orders;
using Domain.Entities.OrderTransactions;
using Domain.Entities.Users;
using Domain.Entities.Vouchers;
using System;
using Xunit;

namespace SeeSharp.EC.Application.UnitTest.OrderTransactionTest;

public class OrderTransactionTests
{
    [Fact]
    public void NewOrderTransaction_Should_Create_Transaction_With_Valid_Properties()
    {
        // Arrange
        var payerName = PayerName.NewPayerName("John Doe");
        var payerEmail = Email.NewEmail("john@example.com");
        var amount = AmountOfOrderTransaction.NewAmountOfOrderTransaction(100.0m);
        var description = DescriptionOfOrderTransaction.NewDescriptionOfOrderTransaction("Test transaction");
        var paymentMethod = PaymentMethod.Cash;
        var isVoucherUsed = IsVoucherUsed.FromBoolean(true);
        var transactionStatus = TransactionStatus.Pending;
        var voucherId = VoucherId.New();
        var orderId = OrderId.New();
        var billId = BillId.New();

        // Act
        var orderTransaction = OrderTransaction.NewOrderTransaction(
            payerName,
            payerEmail,
            amount,
            description,
            paymentMethod,
            isVoucherUsed,
            transactionStatus,
            voucherId,
            orderId,
            billId);

        // Assert
        Assert.NotNull(orderTransaction);
        Assert.NotNull(orderTransaction.OrderTransactionId);
        Assert.Equal(payerName, orderTransaction.PayerName);
        Assert.Equal(payerEmail, orderTransaction.PayerEmail);
        Assert.Equal(amount, orderTransaction.Amount);
        Assert.Equal(description, orderTransaction.Description);
        Assert.Equal(paymentMethod, orderTransaction.PaymentMethod);
        Assert.Equal(isVoucherUsed, orderTransaction.IsVoucherUsed);
        Assert.Equal(transactionStatus, orderTransaction.TransactionStatus);
        Assert.Equal(voucherId, orderTransaction.VoucherId);
        Assert.Equal(orderId, orderTransaction.OrderId);
        Assert.Equal(billId, orderTransaction.BillId);
    }

    [Fact]
    public void NewOrderTransaction_Should_Handle_Null_Optional_Properties()
    {
        // Arrange
        var amount = AmountOfOrderTransaction.NewAmountOfOrderTransaction(100.0m);
        var description = DescriptionOfOrderTransaction.NewDescriptionOfOrderTransaction("Test transaction");
        var paymentMethod = PaymentMethod.Cash;
        var isVoucherUsed = IsVoucherUsed.FromBoolean(false);
        var transactionStatus = TransactionStatus.Pending;
        var orderId = OrderId.New();

        // Act
        var orderTransaction = OrderTransaction.NewOrderTransaction(
            null, // null payerName
            null, // null payerEmail
            amount,
            description,
            paymentMethod,
            isVoucherUsed,
            transactionStatus,
            null, // null voucherId
            orderId,
            null); // null billId

        // Assert
        Assert.NotNull(orderTransaction);
        Assert.Equal(PayerName.Empty, orderTransaction.PayerName);
        Assert.Equal(Email.Empty, orderTransaction.PayerEmail);
        Assert.Null(orderTransaction.VoucherId);
        Assert.Null(orderTransaction.BillId);
    }

    [Fact]
    public void ChangeTransactionStatus_Should_Update_Status()
    {
        // Arrange
        var orderTransaction = CreateSampleOrderTransaction();
        var newStatus = TransactionStatus.Completed;

        // Act
        orderTransaction.ChangeTransactionStatus(newStatus);

        // Assert
        Assert.Equal(newStatus, orderTransaction.TransactionStatus);
    }

    [Theory]
    [InlineData(TransactionStatus.Pending)]
    [InlineData(TransactionStatus.Processing)]
    [InlineData(TransactionStatus.Completed)]
    [InlineData(TransactionStatus.Failed)]
    public void ChangeTransactionStatus_Should_Set_Given_Status(TransactionStatus status)
    {
        // Arrange
        var orderTransaction = CreateSampleOrderTransaction();

        // Act
        orderTransaction.ChangeTransactionStatus(status);

        // Assert
        Assert.Equal(status, orderTransaction.TransactionStatus);
    }

    private OrderTransaction CreateSampleOrderTransaction()
    {
        var amount = AmountOfOrderTransaction.NewAmountOfOrderTransaction(100.0m);
        var description = DescriptionOfOrderTransaction.NewDescriptionOfOrderTransaction("Test transaction");
        var paymentMethod = PaymentMethod.Cash;
        var isVoucherUsed = IsVoucherUsed.FromBoolean(false);
        var transactionStatus = TransactionStatus.Pending;
        var orderId = OrderId.New();

        return OrderTransaction.NewOrderTransaction(
            null,
            null,
            amount,
            description,
            paymentMethod,
            isVoucherUsed,
            transactionStatus,
            null,
            orderId,
            null);
    }
} 