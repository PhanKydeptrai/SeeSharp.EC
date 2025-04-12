using Domain.Entities.CustomerVouchers;
using System;
using System.Linq;
using Xunit;

namespace SeeSharp.EC.Application.UnitTest.CustomerVoucherTest;

public class CustomerVoucherValueObjectsTests
{
    #region CustomerVoucherId Tests
    
    [Fact]
    public void CustomerVoucherId_New_Should_Create_New_Id_With_Valid_Guid()
    {
        // Act
        var id = CustomerVoucherId.New();

        // Assert
        Assert.NotEqual(Guid.Empty, id.Value);
    }

    [Fact]
    public void CustomerVoucherId_FromGuid_Should_Create_Id_With_Specified_Guid()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var id = CustomerVoucherId.FromGuid(guid);

        // Assert
        Assert.Equal(guid, id.Value);
    }

    [Fact]
    public void CustomerVoucherId_FromString_Should_Create_Id_With_Parsed_Guid()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var guidString = guid.ToString();

        // Act
        var id = CustomerVoucherId.FromString(guidString);

        // Assert
        Assert.Equal(guid, id.Value);
    }

    [Fact]
    public void CustomerVoucherId_Empty_Should_Have_Empty_Guid()
    {
        // Act
        var emptyId = CustomerVoucherId.Empty;

        // Assert
        Assert.Equal(Guid.Empty, emptyId.Value);
    }

    [Fact]
    public void CustomerVoucherId_ToString_Should_Return_Guid_String_Representation()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var id = CustomerVoucherId.FromGuid(guid);

        // Act
        var result = id.ToString();

        // Assert
        Assert.Equal(guid.ToString(), result);
    }

    [Fact]
    public void CustomerVoucherId_ToUlid_Should_Convert_To_Ulid()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var id = CustomerVoucherId.FromGuid(guid);

        // Act
        var ulid = id.ToUlid();

        // Assert
        Assert.Equal(guid, ulid.ToGuid());
    }

    [Fact]
    public void CustomerVoucherId_ImplicitOperator_Should_Convert_To_Guid()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var id = CustomerVoucherId.FromGuid(guid);

        // Act
        Guid result = id;

        // Assert
        Assert.Equal(guid, result);
    }

    [Fact]
    public void CustomerVoucherId_GetAtomicValues_Should_Return_Value()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var id = CustomerVoucherId.FromGuid(guid);

        // Act
        var atomicValues = id.GetAtomicValues().ToList();

        // Assert
        Assert.Single(atomicValues);
        Assert.Equal(guid, atomicValues[0]);
    }

    //[Fact]
    //public void CustomerVoucherId_Equality_Should_Work_Correctly()
    //{
    //    // Arrange
    //    var guid = Guid.NewGuid();
    //    var id1 = CustomerVoucherId.FromGuid(guid);
    //    var id2 = CustomerVoucherId.FromGuid(guid);
    //    var id3 = CustomerVoucherId.New();

    //    // Assert
    //    Assert.Equal(id1, id2);
    //    Assert.NotEqual(id1, id3);
    //    Assert.True(id1 == id2);
    //    Assert.False(id1 == id3);
    //    Assert.False(id1 != id2);
    //    Assert.True(id1 != id3);
    //}

    #endregion
    
    #region CustomerVoucherQuantity Tests
    
    [Fact]
    public void CustomerVoucherQuantity_NewCustomerVoucherQuantity_Should_Create_Valid_Instance()
    {
        // Arrange
        var value = 10;

        // Act
        var quantity = CustomerVoucherQuantity.NewCustomerVoucherQuantity(value);

        // Assert
        Assert.NotNull(quantity);
        Assert.Equal(value, quantity.Value);
    }

    [Fact]
    public void CustomerVoucherQuantity_NewCustomerVoucherQuantity_Should_Accept_Zero()
    {
        // Act
        var quantity = CustomerVoucherQuantity.NewCustomerVoucherQuantity(0);

        // Assert
        Assert.Equal(0, quantity.Value);
    }

    [Fact]
    public void CustomerVoucherQuantity_NewCustomerVoucherQuantity_Should_Throw_On_Negative_Value()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
            CustomerVoucherQuantity.NewCustomerVoucherQuantity(-1));
        
        Assert.Contains("Quantity cannot be negative", exception.Message);
    }

    [Fact]
    public void CustomerVoucherQuantity_FromInt_Should_Create_Instance_Without_Validation()
    {
        // Arrange
        var value = 10;

        // Act
        var quantity = CustomerVoucherQuantity.FromInt(value);

        // Assert
        Assert.Equal(value, quantity.Value);
    }

    [Fact]
    public void CustomerVoucherQuantity_GetAtomicValues_Should_Return_Value()
    {
        // Arrange
        var value = 10;
        var quantity = CustomerVoucherQuantity.NewCustomerVoucherQuantity(value);

        // Act
        var atomicValues = quantity.GetAtomicValues().ToList();

        // Assert
        Assert.Single(atomicValues);
        Assert.Equal(value, atomicValues[0]);
    }

    [Fact]
    public void CustomerVoucherQuantity_Equality_Should_Work_Correctly()
    {
        // Arrange
        var quantity1 = CustomerVoucherQuantity.NewCustomerVoucherQuantity(10);
        var quantity2 = CustomerVoucherQuantity.NewCustomerVoucherQuantity(10);
        var quantity3 = CustomerVoucherQuantity.NewCustomerVoucherQuantity(20);

        // Assert
        Assert.Equal(quantity1, quantity2);
        Assert.NotEqual(quantity1, quantity3);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(1, true)]
    [InlineData(100, true)]
    [InlineData(int.MaxValue, true)]
    [InlineData(-1, false)]
    [InlineData(-100, false)]
    [InlineData(int.MinValue, false)]
    public void CustomerVoucherQuantity_NewCustomerVoucherQuantity_Should_Validate_Input(int value, bool isValid)
    {
        if (isValid)
        {
            // Act
            var quantity = CustomerVoucherQuantity.NewCustomerVoucherQuantity(value);
            
            // Assert
            Assert.Equal(value, quantity.Value);
        }
        else
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => 
                CustomerVoucherQuantity.NewCustomerVoucherQuantity(value));
        }
    }

    #endregion
} 