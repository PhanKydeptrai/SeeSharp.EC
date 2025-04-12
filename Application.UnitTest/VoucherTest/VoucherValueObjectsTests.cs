using Domain.Entities.Vouchers;
using Xunit;

namespace SeeSharp.EC.Application.UnitTest.VoucherTest;

public class VoucherValueObjectsTests
{
    [Theory]
    [InlineData("Summer Sale", true)]
    [InlineData("", false)] // Empty
    [InlineData("   ", false)] // Whitespace
    [InlineData("a", true)] // Minimum length
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", true)] // Maximum length (50 chars)
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", false)] // Too long (51 chars)
    public void VoucherName_Validation_Should_Work_Correctly(string name, bool isValid)
    {
        if (isValid)
        {
            var voucherName = VoucherName.NewVoucherName(name);
            Assert.NotNull(voucherName);
            Assert.Equal(name, voucherName.Value);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => VoucherName.NewVoucherName(name));
        }
    }

    [Fact]
    public void VoucherName_FromString_Should_Work_Correctly()
    {
        var name = "Summer Sale";
        var voucherName = VoucherName.FromString(name);
        Assert.NotNull(voucherName);
        Assert.Equal(name, voucherName.Value);
    }

    [Theory]
    [InlineData("SUMMER50", true)]
    [InlineData("", false)] // Empty
    [InlineData("   ", false)] // Whitespace
    [InlineData("a", true)] // Minimum length
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", true)] // Maximum length (50 chars)
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", false)] // Too long (51 chars)
    public void VoucherCode_Validation_Should_Work_Correctly(string code, bool isValid)
    {
        if (isValid)
        {
            var voucherCode = VoucherCode.NewVoucherName(code);
            Assert.NotNull(voucherCode);
            Assert.Equal(code, voucherCode.Value);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => VoucherCode.NewVoucherName(code));
        }
    }

    [Fact]
    public void VoucherCode_FromString_Should_Work_Correctly()
    {
        var code = "SUMMER50";
        var voucherCode = VoucherCode.FromString(code);
        Assert.NotNull(voucherCode);
        Assert.Equal(code, voucherCode.Value);
    }

    [Theory]
    [InlineData("Get 50% off on all summer items", true)]
    [InlineData("", false)] // Empty
    [InlineData("   ", false)] // Whitespace
    [InlineData("a", true)] // Minimum length
    [InlineData(
        "Loremipsumdolorsitamet,consecteturadipiscing elit.tate velit esse cillum dolore eu fugiat nulla pariatur.",
        true)] // Long but valid (under 255 chars)
    [InlineData(
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
        false)] // Too long (over 255 chars)
    public void VoucherDescription_Validation_Should_Work_Correctly(string description, bool isValid)
    {
        if (isValid)
        {
            var voucherDescription = VoucherDescription.NewVoucherDescription(description);
            Assert.NotNull(voucherDescription);
            Assert.Equal(description, voucherDescription.Value);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => VoucherDescription.NewVoucherDescription(description));
        }
    }

    [Fact]
    public void VoucherDescription_FromString_Should_Work_Correctly()
    {
        var description = "Get 50% off on all summer items";
        var voucherDescription = VoucherDescription.FromString(description);
        Assert.NotNull(voucherDescription);
        Assert.Equal(description, voucherDescription.Value);
    }

    [Theory]
    [InlineData(0, true)] // No minimum
    [InlineData(100, true)] // Positive value
    [InlineData(1000, true)] // Large value
    [InlineData(-1, false)] // Negative value
    public void MinimumOrderAmount_Validation_Should_Work_Correctly(decimal amount, bool isValid)
    {
        if (isValid)
        {
            var minimumOrderAmount = MinimumOrderAmount.NewMaxDiscountAmount(amount);
            Assert.NotNull(minimumOrderAmount);
            Assert.Equal(amount, minimumOrderAmount.Value);
        }
        else
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => MinimumOrderAmount.NewMaxDiscountAmount(amount));
        }
    }

    [Fact]
    public void MinimumOrderAmount_FromDecimal_Should_Work_Correctly()
    {
        var amount = 100m;
        var minimumOrderAmount = MinimumOrderAmount.FromDecimal(amount);
        Assert.NotNull(minimumOrderAmount);
        Assert.Equal(amount, minimumOrderAmount.Value);
    }

    [Theory]
    [InlineData(0, true)] // No maximum
    [InlineData(100, true)] // Positive value
    [InlineData(1000, true)] // Large value
    [InlineData(-1, false)] // Negative value
    public void MaximumDiscountAmount_Validation_Should_Work_Correctly(decimal amount, bool isValid)
    {
        if (isValid)
        {
            var maximumDiscountAmount = MaximumDiscountAmount.NewMaxDiscountAmount(amount);
            Assert.NotNull(maximumDiscountAmount);
            Assert.Equal(amount, maximumDiscountAmount.Value);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => MaximumDiscountAmount.NewMaxDiscountAmount(amount));
        }
    }

    [Fact]
    public void MaximumDiscountAmount_FromDecimal_Should_Work_Correctly()
    {
        var amount = 100m;
        var maximumDiscountAmount = MaximumDiscountAmount.FromDecimal(amount);
        Assert.NotNull(maximumDiscountAmount);
        Assert.Equal(amount, maximumDiscountAmount.Value);
    }

    [Theory]
    [InlineData(0, true)] // 0%
    [InlineData(50, true)] // 50%
    [InlineData(100, true)] // 100%
    [InlineData(-1, false)] // Negative value
    [InlineData(101, false)] // Over 100%
    public void PercentageDiscount_Validation_Should_Work_Correctly(int percentage, bool isValid)
    {
        if (isValid)
        {
            var percentageDiscount = PercentageDiscount.NewPercentageDiscount(percentage);
            Assert.NotNull(percentageDiscount);
            Assert.Equal(percentage, percentageDiscount.Value);
        }
        else
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => PercentageDiscount.NewPercentageDiscount(percentage));
        }
    }

    [Fact]
    public void PercentageDiscount_FromInt_Should_Work_Correctly()
    {
        var percentage = 50;
        var percentageDiscount = PercentageDiscount.FromInt(percentage);
        Assert.NotNull(percentageDiscount);
        Assert.Equal(percentage, percentageDiscount.Value);
    }

    [Theory]
    [InlineData(Status.Active)]
    [InlineData(Status.InActive)]
    [InlineData(Status.Expired)]
    public void Status_Should_Have_Correct_Values(Status status)
    {
        switch (status)
        {
            case Status.InActive:
                Assert.Equal(0, (int)status);
                break;
            case Status.Active:
                Assert.Equal(1, (int)status);
                break;
            case Status.Expired:
                Assert.Equal(2, (int)status);
                break;
        }
    }

    [Fact]
    public void Status_Should_Have_All_Expected_Values()
    {
        var values = Enum.GetValues(typeof(Status)).Cast<Status>();
        Assert.Equal(3, values.Count());
        Assert.Contains(Status.InActive, values);
        Assert.Contains(Status.Active, values);
        Assert.Contains(Status.Expired, values);
    }

    [Theory]
    [InlineData(VoucherType.Direct)]
    [InlineData(VoucherType.Percentage)]
    public void VoucherType_Should_Have_Correct_Values(VoucherType type)
    {
        switch (type)
        {
            case VoucherType.Direct:
                Assert.Equal(0, (int)type);
                break;
            case VoucherType.Percentage:
                Assert.Equal(1, (int)type);
                break;
        }
    }

    [Fact]
    public void VoucherType_Should_Have_All_Expected_Values()
    {
        var values = Enum.GetValues(typeof(VoucherType)).Cast<VoucherType>();
        Assert.Equal(2, values.Count());
        Assert.Contains(VoucherType.Direct, values);
        Assert.Contains(VoucherType.Percentage, values);
    }
} 