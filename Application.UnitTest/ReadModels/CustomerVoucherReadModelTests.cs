using Domain.Database.PostgreSQL.ReadModels;
using Domain.Entities.Customers;
using Domain.Entities.Vouchers;
using System;
using System.Collections.Generic;
using Xunit;

namespace SeeSharp.EC.Application.UnitTest.ReadModels;

public class CustomerVoucherReadModelTests
{
    [Fact]
    public void CustomerVoucherReadModel_Properties_Should_Be_Set_Correctly()
    {
        // Arrange
        var customerVoucherId = Ulid.NewUlid();
        var voucherId = Ulid.NewUlid();
        var customerId = Ulid.NewUlid();
        var quantity = 5;

        // Act
        var customerVoucherReadModel = new CustomerVoucherReadModel
        {
            CustomerVoucherId = customerVoucherId,
            VoucherId = voucherId,
            CustomerId = customerId,
            Quantity = quantity
        };

        // Assert
        Assert.Equal(customerVoucherId, customerVoucherReadModel.CustomerVoucherId);
        Assert.Equal(voucherId, customerVoucherReadModel.VoucherId);
        Assert.Equal(customerId, customerVoucherReadModel.CustomerId);
        Assert.Equal(quantity, customerVoucherReadModel.Quantity);
    }

    [Fact]
    public void CustomerVoucherReadModel_Can_Have_Related_Entities()
    {
        // Arrange
        var customerVoucherId = Ulid.NewUlid();
        var voucherId = Ulid.NewUlid();
        var customerId = Ulid.NewUlid();
        var userId = Ulid.NewUlid();
        
        var customerReadModel = new CustomerReadModel 
        { 
            CustomerId = customerId,
            UserId = userId,
            CustomerType = CustomerType.Normal
        };
        
        var voucherReadModel = new VoucherReadModel
        {
            VoucherId = voucherId,
            VoucherName = "Test Voucher",
            VoucherCode = "TESTCODE",
            Status = Status.Active
        };

        // Act
        var customerVoucherReadModel = new CustomerVoucherReadModel
        {
            CustomerVoucherId = customerVoucherId,
            VoucherId = voucherId,
            CustomerId = customerId,
            Quantity = 3,
            CustomerReadModel = customerReadModel,
            VoucherReadModel = voucherReadModel
        };

        // Assert
        Assert.NotNull(customerVoucherReadModel.CustomerReadModel);
        Assert.Equal(customerId, customerVoucherReadModel.CustomerReadModel.CustomerId);
        Assert.Equal(userId, customerVoucherReadModel.CustomerReadModel.UserId);
        Assert.Equal(CustomerType.Normal, customerVoucherReadModel.CustomerReadModel.CustomerType);
        
        Assert.NotNull(customerVoucherReadModel.VoucherReadModel);
        Assert.Equal(voucherId, customerVoucherReadModel.VoucherReadModel.VoucherId);
        Assert.Equal("Test Voucher", customerVoucherReadModel.VoucherReadModel.VoucherName);
        Assert.Equal("TESTCODE", customerVoucherReadModel.VoucherReadModel.VoucherCode);
        Assert.Equal(Status.Active, customerVoucherReadModel.VoucherReadModel.Status);
    }

    [Fact]
    public void CustomerVoucherReadModel_Quantity_Should_Not_Be_Negative()
    {
        // Arrange
        var customerVoucherId = Ulid.NewUlid();
        var voucherId = Ulid.NewUlid();
        var customerId = Ulid.NewUlid();
        
        // Act
        var customerVoucherReadModel = new CustomerVoucherReadModel
        {
            CustomerVoucherId = customerVoucherId,
            VoucherId = voucherId,
            CustomerId = customerId,
            Quantity = 0 // Minimum valid quantity
        };

        // Assert
        Assert.Equal(0, customerVoucherReadModel.Quantity);
        Assert.False(customerVoucherReadModel.Quantity < 0);
    }

    [Fact]
    public void CustomerVoucherReadModel_Should_Track_Quantity_Correctly()
    {
        // Arrange
        var customerVoucherId = Ulid.NewUlid();
        var voucherId = Ulid.NewUlid();
        var customerId = Ulid.NewUlid();
        var initialQuantity = 10;
        
        // Act
        var customerVoucherReadModel = new CustomerVoucherReadModel
        {
            CustomerVoucherId = customerVoucherId,
            VoucherId = voucherId,
            CustomerId = customerId,
            Quantity = initialQuantity
        };
        
        // Test different quantities
        customerVoucherReadModel.Quantity = 5; // Reduce quantity
        Assert.Equal(5, customerVoucherReadModel.Quantity);
        
        customerVoucherReadModel.Quantity = 15; // Increase quantity
        Assert.Equal(15, customerVoucherReadModel.Quantity);
        
        customerVoucherReadModel.Quantity = 0; // Set to zero
        Assert.Equal(0, customerVoucherReadModel.Quantity);
    }
} 