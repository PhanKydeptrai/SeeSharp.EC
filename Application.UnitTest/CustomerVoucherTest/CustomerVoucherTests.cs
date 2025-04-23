using Domain.Entities.Customers;
using Domain.Entities.CustomerVouchers;
using Domain.Entities.Users;
using Domain.Entities.Vouchers;
using System;
using Xunit;

namespace SeeSharp.EC.Application.UnitTest.CustomerVoucherTest;

public class CustomerVoucherTests
{
    [Fact]
    public void NewCustomerVoucher_Should_Create_CustomerVoucher_With_Valid_Properties()
    {
        // Arrange
        var voucherId = VoucherId.New();
        var customerId = CustomerId.New();
        var quantity = CustomerVoucherQuantity.NewCustomerVoucherQuantity(5);

        // Act
        var customerVoucher = CustomerVoucher.NewCustomerVoucher(
            voucherId,
            customerId,
            quantity);

        // Assert
        Assert.NotNull(customerVoucher);
        Assert.NotNull(customerVoucher.CustomerVoucherId);
        Assert.Equal(voucherId, customerVoucher.VoucherId);
        Assert.Equal(customerId, customerVoucher.CustomerId);
        Assert.Equal(quantity, customerVoucher.Quantity);
    }

    [Fact]
    public void ChangeCustomerVoucherQuantity_Should_Update_Quantity()
    {
        // Arrange
        var voucherId = VoucherId.New();
        var customerId = CustomerId.New();
        var initialQuantity = CustomerVoucherQuantity.NewCustomerVoucherQuantity(5);
        var customerVoucher = CustomerVoucher.NewCustomerVoucher(
            voucherId,
            customerId,
            initialQuantity);

        var newQuantity = CustomerVoucherQuantity.NewCustomerVoucherQuantity(10);

        // Act
        customerVoucher.ChangeCustomerVoucherQuantity(newQuantity);

        // Assert
        Assert.Equal(newQuantity, customerVoucher.Quantity);
        Assert.Equal(10, customerVoucher.Quantity.Value);
    }

    [Fact]
    public void CustomerVoucher_Should_Be_Able_To_Set_Related_Entities()
    {
        // Arrange
        var voucherId = VoucherId.New();
        var customerId = CustomerId.New();
        var quantity = CustomerVoucherQuantity.NewCustomerVoucherQuantity(5);
        
        var voucher = CreateSampleVoucher(voucherId);
        var customer = CreateSampleCustomer(customerId);

        // Act
        var customerVoucher = CustomerVoucher.NewCustomerVoucher(
            voucherId,
            customerId,
            quantity);
        
        // Set navigation properties through reflection since they're private setters
        var voucherProperty = typeof(CustomerVoucher).GetProperty("Voucher");
        voucherProperty?.SetValue(customerVoucher, voucher);
        
        var customerProperty = typeof(CustomerVoucher).GetProperty("Customer");
        customerProperty?.SetValue(customerVoucher, customer);

        // Assert
        Assert.NotNull(customerVoucher.Voucher);
        Assert.Equal(voucherId, customerVoucher.Voucher.VoucherId);
        
        Assert.NotNull(customerVoucher.Customer);
        Assert.Equal(customerId, customerVoucher.Customer.CustomerId);
    }

    [Fact]
    public void CustomerVoucherQuantity_Should_Not_Accept_Negative_Values()
    {
        // Arrange & Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
            CustomerVoucherQuantity.NewCustomerVoucherQuantity(-1));
        
        Assert.Contains("Quantity cannot be negative", exception.Message);
    }

    [Fact]
    public void CustomerVoucherQuantity_Should_Accept_Zero_Value()
    {
        // Arrange & Act
        var quantity = CustomerVoucherQuantity.NewCustomerVoucherQuantity(0);
        
        // Assert
        Assert.Equal(0, quantity.Value);
    }

    private Voucher CreateSampleVoucher(VoucherId voucherId)
    {
        // Use reflection to create a Voucher since it has a private constructor
        var voucherType = typeof(Voucher);
        var constructor = voucherType.GetConstructors(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)[0];
        
        return (Voucher)constructor.Invoke(new object[] {
            voucherId,
            VoucherName.NewVoucherName("Test Voucher"),
            VoucherCode.NewVoucherName("TESTCODE"),
            VoucherType.Percentage,
            PercentageDiscount.NewPercentageDiscount(10),
            MaximumDiscountAmount.NewMaxDiscountAmount(100),
            MinimumOrderAmount.NewMaxDiscountAmount(50),
            DateTime.Now,
            DateTime.Now.AddDays(30),
            VoucherDescription.NewVoucherDescription("Test Description"),
            Status.Active
        });
    }

    private Customer CreateSampleCustomer(CustomerId customerId)
    {
        // Use reflection to create a Customer since it might have a private constructor
        var customerType = typeof(Customer);
        var constructor = customerType.GetConstructors(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)[0];
        
        return (Customer)constructor.Invoke(new object[] {
            customerId,
            UserId.FromGuid(Guid.NewGuid()),
            CustomerType.Normal
        });
    }
} 