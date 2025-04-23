using Domain.Entities.Products;
using Xunit;

namespace SeeSharp.EC.Application.UnitTest.ProductTest;

public class ProductValueObjectsTests
{
    [Theory]
    [InlineData("Product Name", true)]
    [InlineData("", false)] // Empty
    [InlineData("   ", false)] // Whitespace
    [InlineData("a", true)] // Minimum length
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", true)] // Maximum length (50 chars)
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", false)] // Too long (51 chars)
    public void ProductName_Validation_Should_Work_Correctly(string productName, bool isValid)
    {
        if (isValid)
        {
            var nameValue = ProductName.NewProductName(productName);
            Assert.NotNull(nameValue);
            Assert.Equal(productName, nameValue.Value);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => ProductName.NewProductName(productName));
        }
    }


    [Theory]
    [InlineData(ProductStatus.InStock)]
    [InlineData(ProductStatus.OutOfStock)]
    [InlineData(ProductStatus.Discontinued)]
    public void ProductStatus_Should_Have_Correct_Values(ProductStatus status)
    {
        switch (status)
        {
            case ProductStatus.InStock:
                Assert.Equal(0, (int)status);
                break;
            case ProductStatus.OutOfStock:
                Assert.Equal(1, (int)status);
                break;
            case ProductStatus.Discontinued:
                Assert.Equal(2, (int)status);
                break;
        }
    }

    [Fact]
    public void ProductStatus_Should_Have_All_Expected_Values()
    {
        var values = Enum.GetValues(typeof(ProductStatus)).Cast<ProductStatus>();
        Assert.Equal(3, values.Count());
        Assert.Contains(ProductStatus.InStock, values);
        Assert.Contains(ProductStatus.OutOfStock, values);
        Assert.Contains(ProductStatus.Discontinued, values);
    }
} 