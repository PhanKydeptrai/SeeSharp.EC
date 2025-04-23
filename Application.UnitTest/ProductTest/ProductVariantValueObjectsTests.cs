using Domain.Entities.ProductVariants;
using Xunit;

namespace SeeSharp.EC.Application.UnitTest.ProductTest;

public class ProductVariantValueObjectsTests
{
    [Theory]
    [InlineData("#FF0000", true)] // Red
    [InlineData("#00FF00", true)] // Green
    [InlineData("#0000FF", true)] // Blue
    [InlineData("#123456", true)] // Valid hex
    [InlineData("FF0000", false)] // Missing #
    [InlineData("#FF00", false)] // Too short
    [InlineData("#FF00000", false)] // Too long
    [InlineData("", false)] // Empty
    [InlineData("   ", false)] // Whitespace
    public void ColorCode_Validation_Should_Work_Correctly(string colorCode, bool isValid)
    {
        if (isValid)
        {
            var colorValue = ColorCode.Create(colorCode);
            Assert.NotNull(colorValue);
            Assert.Equal(colorCode, colorValue.Value);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => ColorCode.Create(colorCode));
        }
    }



    [Theory]
    [InlineData("Variant Name", true)]
    [InlineData("", false)] // Empty
    [InlineData("   ", false)] // Whitespace
    [InlineData("a", true)] // Minimum length
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", true)] // Maximum length (50 chars)
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", false)] // Too long (51 chars)
    public void VariantName_Validation_Should_Work_Correctly(string variantName, bool isValid)
    {
        if (isValid)
        {
            var nameValue = VariantName.NewVariantName(variantName);
            Assert.NotNull(nameValue);
            Assert.Equal(variantName, nameValue.Value);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => VariantName.NewVariantName(variantName));
        }
    }

    [Theory]
    [InlineData(0.0, true)]
    [InlineData(100.0, true)]
    [InlineData(999.99, true)]
    [InlineData(-1.0, false)] // Negative
    public void ProductVariantPrice_Validation_Should_Work_Correctly(decimal price, bool isValid)
    {
        if (isValid)
        {
            var priceValue = ProductVariantPrice.NewProductPrice(price);
            Assert.NotNull(priceValue);
            Assert.Equal(price, priceValue.Value);
        }
        else
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ProductVariantPrice.NewProductPrice(price));
        }
    }

    [Theory]
    [InlineData(ProductVariantStatus.InStock)]
    [InlineData(ProductVariantStatus.OutOfStock)]
    [InlineData(ProductVariantStatus.Discontinued)]
    public void ProductVariantStatus_Should_Have_Correct_Values(ProductVariantStatus status)
    {
        switch (status)
        {
            case ProductVariantStatus.InStock:
                Assert.Equal(0, (int)status);
                break;
            case ProductVariantStatus.OutOfStock:
                Assert.Equal(1, (int)status);
                break;
            case ProductVariantStatus.Discontinued:
                Assert.Equal(2, (int)status);
                break;
        }
    }

    [Fact]
    public void ProductVariantStatus_Should_Have_All_Expected_Values()
    {
        var values = Enum.GetValues(typeof(ProductVariantStatus)).Cast<ProductVariantStatus>();
        Assert.Equal(3, values.Count());
        Assert.Contains(ProductVariantStatus.InStock, values);
        Assert.Contains(ProductVariantStatus.OutOfStock, values);
        Assert.Contains(ProductVariantStatus.Discontinued, values);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsBaseVariant_Should_Work_Correctly(bool value)
    {
        var isBaseVariant = IsBaseVariant.FromBoolean(value);
        Assert.Equal(value, isBaseVariant.Value);
    }

    [Fact]
    public void IsBaseVariant_Equality_Should_Work_Correctly()
    {
        var true1 = IsBaseVariant.True;
        var true2 = IsBaseVariant.True;
        var false1 = IsBaseVariant.False;

        Assert.True(true1.Equals(true2));
        Assert.False(true1.Equals(false1));
        Assert.True(true1 == true2);
        Assert.False(true1 == false1);
    }

    [Theory]
    [InlineData("Valid description", true)]
    [InlineData("", false)] // Empty
    [InlineData("   ", false)] // Whitespace
    [InlineData("a", true)] // Minimum length
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", true)] // Maximum length (255 chars)
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", false)] // Too long (256 chars)
    public void ProductVariantDescription_Validation_Should_Work_Correctly(string description, bool isValid)
    {
        if (isValid)
        {
            var descValue = ProductVariantDescription.Create(description);
            Assert.NotNull(descValue);
            Assert.Equal(description, descValue.Value);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => ProductVariantDescription.Create(description));
        }
    }
} 