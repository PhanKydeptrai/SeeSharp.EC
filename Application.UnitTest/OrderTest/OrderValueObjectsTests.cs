using Domain.Entities.Orders;
using Xunit;

namespace SeeSharp.EC.Application.UnitTest.OrderTest;

public class OrderValueObjectsTests
{
    [Theory]
    [InlineData(0.0, true)] // Zero value
    [InlineData(100.0, true)] // Positive value
    [InlineData(999999.99, true)] // Large value
    [InlineData(-1.0, false)] // Negative value
    public void OrderTotal_Validation_Should_Work_Correctly(decimal total, bool isValid)
    {
        if (isValid)
        {
            var orderTotal = OrderTotal.NewOrderTotal(total);
            Assert.NotNull(orderTotal);
            Assert.Equal(total, orderTotal.Value);
        }
        else
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => OrderTotal.NewOrderTotal(total));
        }
    }

    [Fact]
    public void OrderTotal_FromDecimal_Should_Work_Correctly()
    {
        var total = 123.45m;
        var orderTotal = OrderTotal.FromDecimal(total);
        Assert.NotNull(orderTotal);
        Assert.Equal(total, orderTotal.Value);
    }

    [Theory]
    [InlineData(OrderPaymentStatus.Waiting)]
    [InlineData(OrderPaymentStatus.Paid)]
    [InlineData(OrderPaymentStatus.Unpaid)]
    public void OrderPaymentStatus_Should_Have_Correct_Values(OrderPaymentStatus status)
    {
        switch (status)
        {
            case OrderPaymentStatus.Waiting:
                Assert.Equal(0, (int)status);
                break;
            case OrderPaymentStatus.Paid:
                Assert.Equal(1, (int)status);
                break;
            case OrderPaymentStatus.Unpaid:
                Assert.Equal(2, (int)status);
                break;
        }
    }

    [Fact]
    public void OrderPaymentStatus_Should_Have_All_Expected_Values()
    {
        var values = Enum.GetValues(typeof(OrderPaymentStatus)).Cast<OrderPaymentStatus>();
        Assert.Equal(3, values.Count());
        Assert.Contains(OrderPaymentStatus.Waiting, values);
        Assert.Contains(OrderPaymentStatus.Paid, values);
        Assert.Contains(OrderPaymentStatus.Unpaid, values);
    }

    [Theory]
    [InlineData(OrderStatus.Waiting)]
    [InlineData(OrderStatus.New)]
    [InlineData(OrderStatus.Processing)]
    [InlineData(OrderStatus.Shipped)]
    [InlineData(OrderStatus.Delivered)]
    [InlineData(OrderStatus.Cancelled)]
    public void OrderStatus_Should_Have_Correct_Values(OrderStatus status)
    {
        switch (status)
        {
            case OrderStatus.Waiting:
                Assert.Equal(0, (int)status);
                break;
            case OrderStatus.New:
                Assert.Equal(1, (int)status);
                break;
            case OrderStatus.Processing:
                Assert.Equal(2, (int)status);
                break;
            case OrderStatus.Shipped:
                Assert.Equal(3, (int)status);
                break;
            case OrderStatus.Delivered:
                Assert.Equal(4, (int)status);
                break;
            case OrderStatus.Cancelled:
                Assert.Equal(5, (int)status);
                break;
        }
    }

    [Fact]
    public void OrderStatus_Should_Have_All_Expected_Values()
    {
        var values = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>();
        Assert.Equal(6, values.Count());
        Assert.Contains(OrderStatus.Waiting, values);
        Assert.Contains(OrderStatus.New, values);
        Assert.Contains(OrderStatus.Processing, values);
        Assert.Contains(OrderStatus.Shipped, values);
        Assert.Contains(OrderStatus.Delivered, values);
        Assert.Contains(OrderStatus.Cancelled, values);
    }
} 