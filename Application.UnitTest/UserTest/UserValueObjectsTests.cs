using Domain.Entities.Users;
using Xunit;

namespace SeeSharp.EC.Application.UnitTest.UserTest;

public class UserValueObjectsTests
{
    [Theory]
    [InlineData("test@example.com", true)]
    [InlineData("invalid-email", false)]
    [InlineData("test@.com", false)]
    [InlineData("test@domain", false)]
    [InlineData("", false)] // Empty
    [InlineData("a@b.c", true)] // Minimum valid email
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa@b.c", false)] // Too long (50+ chars)
    public void Email_Validation_Should_Work_Correctly(string email, bool isValid)
    {
        if (isValid)
        {
            var emailValue = Email.NewEmail(email);
            Assert.NotNull(emailValue);
            Assert.Equal(email, emailValue.Value);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => Email.NewEmail(email));
        }
    }

    [Theory]
    [InlineData("0123456789", true)] // 10 digits
    [InlineData("1234567890", true)] // 10 digitsadws
    [InlineData("123", false)] // Too short
    [InlineData("01234567890", false)] // Too long
    [InlineData("invalid", false)] // Non-numeric
    [InlineData("", false)] // Empty
    public void PhoneNumber_Validation_Should_Work_Correctly(string phoneNumber, bool isValid)
    {
        if (isValid)
        {
            var phoneValue = PhoneNumber.NewPhoneNumber(phoneNumber);
            Assert.NotNull(phoneValue);
            Assert.Equal(phoneNumber, phoneValue.Value);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => PhoneNumber.NewPhoneNumber(phoneNumber));
        }
    }

    [Theory]
    [InlineData("validusername", true)]
    [InlineData("user123", true)]
    [InlineData("", false)] // Empty
    [InlineData(" ", false)]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", true)] // Maximum length
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", false)] // Too long
    public void UserName_Validation_Should_Work_Correctly(string userName, bool isValid)
    {
        if (isValid)
        {
            var userNameValue = UserName.NewUserName(userName);
            Assert.NotNull(userNameValue);
            Assert.Equal(userName, userNameValue.Value);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => UserName.NewUserName(userName));
        }
    }

    [Theory]
    [InlineData("0123456789012345678901234567890123456789012345678901234567890123", true)] // 64 chars
    [InlineData("", false)] // Empty
    [InlineData(" ", false)]
    [InlineData("123", false)] // Too short
    [InlineData("01234567890123456789012345678901234567890123456789012345678901234", false)] // Too long
    public void PasswordHash_Validation_Should_Work_Correctly(string passwordHash, bool isValid)
    {
        if (isValid)
        {
            var hashValue = PasswordHash.NewPasswordHash(passwordHash);
            Assert.NotNull(hashValue);
            Assert.Equal(passwordHash, hashValue.Value);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => PasswordHash.NewPasswordHash(passwordHash));
        }
    }
} 