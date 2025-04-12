using Domain.Utilities.Events.EmployeeEvents;
using System;
using Xunit;

namespace SeeSharp.EC.Application.UnitTest.EventsTest;

public class EmployeeEventsTests
{
    [Fact]
    public void EmployeeChangePasswordEvent_Properties_Should_Be_Set_Correctly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var verificationTokenId = Guid.NewGuid();
        var email = "employee@example.com";
        var messageId = Guid.NewGuid();

        // Act
        var @event = new EmployeeChangePasswordEvent(
            userId,
            verificationTokenId,
            email,
            messageId);

        // Assert
        Assert.Equal(userId, @event.UserId);
        Assert.Equal(verificationTokenId, @event.VerificationTokenId);
        Assert.Equal(email, @event.Email);
        Assert.Equal(messageId, @event.MessageId);
    }

    [Fact]
    public void EmployeeResetPasswordEvent_Properties_Should_Be_Set_Correctly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var email = "employee@example.com";
        var randomPassword = "Password123!";
        var messageId = Guid.NewGuid();

        // Act
        var @event = new EmployeeResetPasswordEvent(
            userId,
            email,
            randomPassword,
            messageId);

        // Assert
        Assert.Equal(userId, @event.UserId);
        Assert.Equal(email, @event.Email);
        Assert.Equal(randomPassword, @event.RandomPassword);
        Assert.Equal(messageId, @event.MessageId);
    }

    [Fact]
    public void EmployeeResetPasswordEmailSendEvent_Properties_Should_Be_Set_Correctly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var verificationTokenId = Guid.NewGuid();
        var email = "employee@example.com";
        var messageId = Guid.NewGuid();

        // Act
        var @event = new EmployeeResetPasswordEmailSendEvent(
            userId,
            verificationTokenId,
            email,
            messageId);

        // Assert
        Assert.Equal(userId, @event.UserId);
        Assert.Equal(verificationTokenId, @event.VerificationTokenId);
        Assert.Equal(email, @event.Email);
        Assert.Equal(messageId, @event.MessageId);
    }

    [Fact]
    public void EmployeeConfirmChangePasswordEvent_Properties_Should_Be_Set_Correctly()
    {
        // Arrange
        var email = "employee@example.com";
        var messageId = Guid.NewGuid();

        // Act
        var @event = new EmployeeConfirmChangePasswordEvent(
            email,
            messageId);

        // Assert
        Assert.Equal(email, @event.Email);
        Assert.Equal(messageId, @event.MessageId);
    }

    [Fact]
    public void SendDefaultPasswordToUserEvent_Properties_Should_Be_Set_Correctly()
    {
        // Arrange
        var email = "employee@example.com";
        var randomPassword = "DefaultPassword123!";
        var messageId = Guid.NewGuid();

        // Act
        var @event = new SendDefaultPasswordToUserEvent(
            email,
            randomPassword,
            messageId);

        // Assert
        Assert.Equal(email, @event.Email);
        Assert.Equal(randomPassword, @event.RandomPassword);
        Assert.Equal(messageId, @event.MessageId);
    }

    [Fact]
    public void EmployeeEvents_With_Same_Values_Should_Be_Equal()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var verificationTokenId = Guid.NewGuid();
        var email = "employee@example.com";
        var messageId = Guid.NewGuid();

        // Act
        var event1 = new EmployeeChangePasswordEvent(userId, verificationTokenId, email, messageId);
        var event2 = new EmployeeChangePasswordEvent(userId, verificationTokenId, email, messageId);

        // Assert - Records have value equality
        Assert.Equal(event1, event2);
        Assert.True(event1 == event2);
    }

    [Fact]
    public void EmployeeEvents_With_Different_Values_Should_Not_Be_Equal()
    {
        // Arrange
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var verificationTokenId = Guid.NewGuid();
        var email = "employee@example.com";
        var messageId = Guid.NewGuid();

        // Act
        var event1 = new EmployeeChangePasswordEvent(userId1, verificationTokenId, email, messageId);
        var event2 = new EmployeeChangePasswordEvent(userId2, verificationTokenId, email, messageId);

        // Assert - Records with different values should not be equal
        Assert.NotEqual(event1, event2);
        Assert.False(event1 == event2);
    }
} 