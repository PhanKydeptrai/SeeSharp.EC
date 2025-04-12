using Domain.Entities.OrderTransactions;
using System;
using System.Linq;
using Xunit;

namespace SeeSharp.EC.Application.UnitTest.OrderTransactionTest;

public class OrderTransactionValueObjectsTests
{
    #region OrderTransactionId Tests
    
    [Fact]
    public void OrderTransactionId_New_Should_Create_New_Id_With_Valid_Guid()
    {
        // Act
        var id = OrderTransactionId.New();

        // Assert
        Assert.NotEqual(Guid.Empty, id.Value);
    }

    [Fact]
    public void OrderTransactionId_FromGuid_Should_Create_Id_With_Specified_Guid()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var id = OrderTransactionId.FromGuid(guid);

        // Assert
        Assert.Equal(guid, id.Value);
    }

    [Fact]
    public void OrderTransactionId_FromString_Should_Create_Id_With_Parsed_Guid()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var guidString = guid.ToString();

        // Act
        var id = OrderTransactionId.FromString(guidString);

        // Assert
        Assert.Equal(guid, id.Value);
    }

    [Fact]
    public void OrderTransactionId_Empty_Should_Have_Empty_Guid()
    {
        // Act
        var emptyId = OrderTransactionId.Empty;

        // Assert
        Assert.Equal(Guid.Empty, emptyId.Value);
    }

    #endregion

    #region PayerName Tests
    
    [Fact]
    public void PayerName_NewPayerName_Should_Create_Valid_Instance()
    {
        // Arrange
        var value = "John Doe";

        // Act
        var payerName = PayerName.NewPayerName(value);

        // Assert
        Assert.NotNull(payerName);
        Assert.Equal(value, payerName.Value);
    }

    [Fact]
    public void PayerName_NewPayerName_Should_Throw_On_Null_Or_Empty_Value()
    {
        // Act & Assert
        var exceptionNull = Assert.Throws<ArgumentNullException>(() => 
            PayerName.NewPayerName(null));
        
        var exceptionEmpty = Assert.Throws<ArgumentNullException>(() => 
            PayerName.NewPayerName(string.Empty));
        
        Assert.Contains("Payer name is empty", exceptionNull.Message);
        Assert.Contains("Payer name is empty", exceptionEmpty.Message);
    }

    [Fact]
    public void PayerName_NewPayerName_Should_Throw_On_Too_Long_Value()
    {
        // Arrange
        var tooLongName = new string('A', PayerName.MaxLengh + 1);

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
            PayerName.NewPayerName(tooLongName));
        
        Assert.Contains("Payer name is too long", exception.Message);
    }

    [Fact]
    public void PayerName_FromString_Should_Create_Instance_Without_Validation()
    {
        // Arrange
        var value = "John Doe";

        // Act
        var payerName = PayerName.FromString(value);

        // Assert
        Assert.Equal(value, payerName.Value);
    }

    [Fact]
    public void PayerName_Empty_Should_Have_EmptyString()
    {
        // Act
        var emptyName = PayerName.Empty;

        // Assert
        Assert.Equal(string.Empty, emptyName.Value);
    }

    [Fact]
    public void PayerName_GetAtomicValues_Should_Return_Value()
    {
        // Arrange
        var value = "John Doe";
        var payerName = PayerName.NewPayerName(value);

        // Act
        var atomicValues = payerName.GetAtomicValues().ToList();

        // Assert
        Assert.Single(atomicValues);
        Assert.Equal(value, atomicValues[0]);
    }

    #endregion

    #region AmountOfOrderTransaction Tests
    
    [Fact]
    public void AmountOfOrderTransaction_NewAmountOfOrderTransaction_Should_Create_Valid_Instance()
    {
        // Arrange
        var value = 100.0m;

        // Act
        var amount = AmountOfOrderTransaction.NewAmountOfOrderTransaction(value);

        // Assert
        Assert.NotNull(amount);
        Assert.Equal(value, amount.Value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void AmountOfOrderTransaction_NewAmountOfOrderTransaction_Should_Throw_On_Invalid_Value(decimal value)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
            AmountOfOrderTransaction.NewAmountOfOrderTransaction(value));
        
        Assert.Contains("Amount must be greater than 0", exception.Message);
    }

    [Fact]
    public void AmountOfOrderTransaction_FromDecimal_Should_Create_Instance_Without_Validation()
    {
        // Arrange
        var value = 100.0m;

        // Act
        var amount = AmountOfOrderTransaction.FromDecimal(value);

        // Assert
        Assert.Equal(value, amount.Value);
    }

    [Fact]
    public void AmountOfOrderTransaction_GetAtomicValues_Should_Return_Value()
    {
        // Arrange
        var value = 100.0m;
        var amount = AmountOfOrderTransaction.NewAmountOfOrderTransaction(value);

        // Act
        var atomicValues = amount.GetAtomicValues().ToList();

        // Assert
        Assert.Single(atomicValues);
        Assert.Equal(value, atomicValues[0]);
    }

    #endregion

    #region DescriptionOfOrderTransaction Tests
    
    [Fact]
    public void DescriptionOfOrderTransaction_NewDescriptionOfOrderTransaction_Should_Create_Valid_Instance()
    {
        // Arrange
        var value = "Test transaction description";

        // Act
        var description = DescriptionOfOrderTransaction.NewDescriptionOfOrderTransaction(value);

        // Assert
        Assert.NotNull(description);
        Assert.Equal(value, description.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void DescriptionOfOrderTransaction_NewDescriptionOfOrderTransaction_Should_Throw_On_Empty_Value(string value)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => 
            DescriptionOfOrderTransaction.NewDescriptionOfOrderTransaction(value));
        
        Assert.Contains("Description must not be empty", exception.Message);
    }

    [Fact]
    public void DescriptionOfOrderTransaction_NewDescriptionOfOrderTransaction_Should_Throw_On_Too_Long_Value()
    {
        // Arrange - Generate a string longer than 255 characters
        var tooLongDescription = new string('A', 256);

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
            DescriptionOfOrderTransaction.NewDescriptionOfOrderTransaction(tooLongDescription));
        
        Assert.Contains("Description must not exceed", exception.Message);
    }

    [Fact]
    public void DescriptionOfOrderTransaction_FromString_Should_Create_Instance_Without_Validation()
    {
        // Arrange
        var value = "Test transaction description";

        // Act
        var description = DescriptionOfOrderTransaction.FromString(value);

        // Assert
        Assert.Equal(value, description.Value);
    }

    [Fact]
    public void DescriptionOfOrderTransaction_Empty_Should_Have_EmptyString()
    {
        // Act
        var emptyDescription = DescriptionOfOrderTransaction.Empty;

        // Assert
        Assert.Equal(string.Empty, emptyDescription.Value);
    }

    [Fact]
    public void DescriptionOfOrderTransaction_GetAtomicValues_Should_Return_Value()
    {
        // Arrange
        var value = "Test transaction description";
        var description = DescriptionOfOrderTransaction.NewDescriptionOfOrderTransaction(value);

        // Act
        var atomicValues = description.GetAtomicValues().ToList();

        // Assert
        Assert.Single(atomicValues);
        Assert.Equal(value, atomicValues[0]);
    }

    #endregion

    #region IsVoucherUsed Tests
    
    [Fact]
    public void IsVoucherUsed_NotUsed_Should_Have_False_Value()
    {
        // Act
        var notUsed = IsVoucherUsed.NotUsed;

        // Assert
        Assert.False(notUsed.Value);
    }

    [Fact]
    public void IsVoucherUsed_Used_Should_Have_True_Value()
    {
        // Act
        var used = IsVoucherUsed.Used;

        // Assert
        Assert.True(used.Value);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsVoucherUsed_FromBoolean_Should_Create_Correct_Instance(bool value)
    {
        // Act
        var isVoucherUsed = IsVoucherUsed.FromBoolean(value);

        // Assert
        Assert.Equal(value, isVoucherUsed.Value);
    }

    [Fact]
    public void IsVoucherUsed_GetAtomicValues_Should_Return_Value()
    {
        // Arrange
        var isVoucherUsed = IsVoucherUsed.Used;

        // Act
        var atomicValues = isVoucherUsed.GetAtomicValues().ToList();

        // Assert
        Assert.Single(atomicValues);
        Assert.Equal(true, atomicValues[0]);
    }

    #endregion

    #region TransactionStatus Tests
    
    [Fact]
    public void TransactionStatus_Should_Have_Expected_Values()
    {
        // Assert
        Assert.Equal(0, (int)TransactionStatus.Pending);
        Assert.Equal(1, (int)TransactionStatus.Processing);
        Assert.Equal(2, (int)TransactionStatus.Completed);
        Assert.Equal(3, (int)TransactionStatus.Failed);
    }

    [Fact]
    public void TransactionStatus_Should_Have_Four_Possible_Values()
    {
        // Act
        var values = Enum.GetValues(typeof(TransactionStatus));

        // Assert
        Assert.Equal(4, values.Length);
    }

    #endregion
} 