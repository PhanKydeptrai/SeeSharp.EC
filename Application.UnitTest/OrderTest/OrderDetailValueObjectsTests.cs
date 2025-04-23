using Domain.Entities.OrderDetails;
using Xunit;

namespace SeeSharp.EC.Application.UnitTest.OrderTest;

public class OrderDetailValueObjectsTests
{
    [Theory]
    [InlineData(0.01, true)] // Minimum valid price
    [InlineData(100.0, true)] // Positive value
    [InlineData(9999.99, true)] // Large value
    [InlineData(0.0, false)] // Zero value - invalid
    [InlineData(-1.0, false)] // Negative value - invalid
    public void OrderDetailUnitPrice_Validation_Should_Work_Correctly(decimal price, bool isValid)
    {
        if (isValid)
        {
            var unitPrice = OrderDetailUnitPrice.NewOrderDetailUnitPrice(price);
            Assert.NotNull(unitPrice);
            Assert.Equal(price, unitPrice.Value);
        }
        else
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => OrderDetailUnitPrice.NewOrderDetailUnitPrice(price));
        }
    }

    [Fact]
    public void OrderDetailUnitPrice_FromDecimal_Should_Work_Correctly()
    {
        var price = 123.45m;
        var unitPrice = OrderDetailUnitPrice.FromDecimal(price);
        Assert.NotNull(unitPrice);
        Assert.Equal(price, unitPrice.Value);
    }

    [Theory]
    [InlineData(1, true)] // Minimum valid quantity
    [InlineData(10, true)] // Small valid quantity
    [InlineData(100, true)] // Large valid quantity
    [InlineData(0, false)] // Zero quantity - invalid
    [InlineData(-1, false)] // Negative quantity - invalid
    public void OrderDetailQuantity_Validation_Should_Work_Correctly(int quantity, bool isValid)
    {
        if (isValid)
        {
            var orderDetailQuantity = OrderDetailQuantity.NewOrderDetailQuantity(quantity);
            Assert.NotNull(orderDetailQuantity);
            Assert.Equal(quantity, orderDetailQuantity.Value);
        }
        else
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => OrderDetailQuantity.NewOrderDetailQuantity(quantity));
        }
    }

    [Fact]
    public void OrderDetailQuantity_FromInt_Should_Work_Correctly()
    {
        var quantity = 5;
        var orderDetailQuantity = OrderDetailQuantity.FromInt(quantity);
        Assert.NotNull(orderDetailQuantity);
        Assert.Equal(quantity, orderDetailQuantity.Value);
    }

    [Fact]
    public void OrderDetailQuantity_And_UnitPrice_Calculate_Total_Correctly()
    {
        var quantity = OrderDetailQuantity.NewOrderDetailQuantity(3);
        var unitPrice = OrderDetailUnitPrice.NewOrderDetailUnitPrice(10.5m);
        
        // Calculate total manually
        var expectedTotal = quantity.Value * unitPrice.Value;
        
        // Verify
        Assert.Equal(31.5m, expectedTotal);
    }
} 