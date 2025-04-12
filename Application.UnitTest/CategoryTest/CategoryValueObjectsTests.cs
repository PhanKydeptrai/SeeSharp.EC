using Domain.Entities.Categories;
using Xunit;

namespace SeeSharp.EC.Application.UnitTest.CategoryTest;

public class CategoryValueObjectsTests
{
    [Theory]
    [InlineData("Category Name", true)]
    [InlineData("", false)] // Empty
    [InlineData("   ", false)] // Whitespace
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", true)] // Maximum length (50 chars)
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", false)] // Too long (51 chars)
    public void CategoryName_Validation_Should_Work_Correctly(string categoryName, bool isValid)
    {
        if (isValid)
        {
            var nameValue = CategoryName.NewCategoryName(categoryName);
            Assert.NotNull(nameValue);
            Assert.Equal(categoryName, nameValue.Value);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => CategoryName.NewCategoryName(categoryName));
        }
    }

    [Theory]
    [InlineData(CategoryStatus.Available)]
    [InlineData(CategoryStatus.Unavailable)]
    [InlineData(CategoryStatus.Deleted)]
    public void CategoryStatus_Should_Have_Correct_Values(CategoryStatus status)
    {
        switch (status)
        {
            case CategoryStatus.Available:
                Assert.Equal(0, (int)status);
                break;
            case CategoryStatus.Unavailable:
                Assert.Equal(1, (int)status);
                break;
            case CategoryStatus.Deleted:
                Assert.Equal(2, (int)status);
                break;
        }
    }

    [Fact]
    public void CategoryStatus_Should_Have_All_Expected_Values()
    {
        var values = Enum.GetValues(typeof(CategoryStatus)).Cast<CategoryStatus>();
        Assert.Equal(3, values.Count());
        Assert.Contains(CategoryStatus.Available, values);
        Assert.Contains(CategoryStatus.Unavailable, values);
        Assert.Contains(CategoryStatus.Deleted, values);
    }
} 