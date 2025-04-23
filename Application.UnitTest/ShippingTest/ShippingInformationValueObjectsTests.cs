using Domain.Entities.ShippingInformations;
using Xunit;

namespace SeeSharp.EC.Application.UnitTest.ShippingTest;

public class ShippingInformationValueObjectsTests
{
    [Theory]
    [InlineData("John Doe", true)]
    [InlineData("", false)] // Empty
    [InlineData("   ", false)] // Whitespace
    [InlineData("a", true)] // Minimum length
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", true)] // Maximum length (50 chars)
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", false)] // Too long (51 chars)
    public void FullName_Validation_Should_Work_Correctly(string name, bool isValid)
    {
        if (isValid)
        {
            var fullName = FullName.NewFullName(name);
            Assert.NotNull(fullName);
            Assert.Equal(name, fullName.Value);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => FullName.NewFullName(name));
        }
    }

    [Fact]
    public void FullName_FromString_Should_Work_Correctly()
    {
        var name = "John Doe";
        var fullName = FullName.FromString(name);
        Assert.NotNull(fullName);
        Assert.Equal(name, fullName.Value);
    }

    [Theory]
    [InlineData("Ho Chi Minh", true)]
    [InlineData("", false)] // Empty
    [InlineData("   ", false)] // Whitespace
    [InlineData("a", true)] // Minimum length
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", true)] // Maximum length (50 chars)
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", false)] // Too long (51 chars)
    public void Province_Validation_Should_Work_Correctly(string province, bool isValid)
    {
        if (isValid)
        {
            var provinceValue = Province.NewProvince(province);
            Assert.NotNull(provinceValue);
            Assert.Equal(province, provinceValue.Value);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => Province.NewProvince(province));
        }
    }

    [Fact]
    public void Province_FromString_Should_Work_Correctly()
    {
        var province = "Ho Chi Minh";
        var provinceValue = Province.FromString(province);
        Assert.NotNull(provinceValue);
        Assert.Equal(province, provinceValue.Value);
    }

    [Theory]
    [InlineData("District 1", true)]
    [InlineData("", false)] // Empty
    [InlineData("   ", false)] // Whitespace
    [InlineData("a", true)] // Minimum length
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", true)] // Maximum length (50 chars)
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", false)] // Too long (51 chars)
    public void District_Validation_Should_Work_Correctly(string district, bool isValid)
    {
        if (isValid)
        {
            var districtValue = District.NewDistrict(district);
            Assert.NotNull(districtValue);
            Assert.Equal(district, districtValue.Value);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => District.NewDistrict(district));
        }
    }

    [Fact]
    public void District_FromString_Should_Work_Correctly()
    {
        var district = "District 1";
        var districtValue = District.FromString(district);
        Assert.NotNull(districtValue);
        Assert.Equal(district, districtValue.Value);
    }

    [Theory]
    [InlineData("Ward 1", true)]
    [InlineData("", false)] // Empty
    [InlineData("   ", false)] // Whitespace
    [InlineData("a", true)] // Minimum length
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", true)] // Maximum length (50 chars)
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", false)] // Too long (51 chars)
    public void Ward_Validation_Should_Work_Correctly(string ward, bool isValid)
    {
        if (isValid)
        {
            var wardValue = Ward.NewDistrict(ward);
            Assert.NotNull(wardValue);
            Assert.Equal(ward, wardValue.Value);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => Ward.NewDistrict(ward));
        }
    }

    [Fact]
    public void Ward_FromString_Should_Work_Correctly()
    {
        var ward = "Ward 1";
        var wardValue = Ward.FromString(ward);
        Assert.NotNull(wardValue);
        Assert.Equal(ward, wardValue.Value);
    }

    [Theory]
    [InlineData("123 Street Name", true)]
    [InlineData("", false)] // Empty
    [InlineData("   ", false)] // Whitespace
    [InlineData("a", true)] // Minimum length
    //[InlineData(
    //    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.", 
    //    true)] // Long but valid (under 255 chars)
    //[InlineData(
    //    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.", 
    //    false)] // Too long (over 255 chars)
    public void SpecificAddress_Validation_Should_Work_Correctly(string address, bool isValid)
    {
        if (isValid)
        {
            var addressValue = SpecificAddress.NewSpecificAddress(address);
            Assert.NotNull(addressValue);
            Assert.Equal(address, addressValue.Value);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => SpecificAddress.NewSpecificAddress(address));
        }
    }

    [Fact]
    public void SpecificAddress_FromString_Should_Work_Correctly()
    {
        var address = "123 Street Name";
        var addressValue = SpecificAddress.FromString(address);
        Assert.NotNull(addressValue);
        Assert.Equal(address, addressValue.Value);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsDefault_Should_Work_Correctly(bool value)
    {
        var isDefault = IsDefault.FromBoolean(value);
        Assert.NotNull(isDefault);
        Assert.Equal(value, isDefault.Value);
    }

    [Fact]
    public void IsDefault_Predefined_Values_Should_Work_Correctly()
    {
        var trueValue = IsDefault.True;
        var falseValue = IsDefault.False;
        
        Assert.True(trueValue.Value);
        Assert.False(falseValue.Value);
    }
} 