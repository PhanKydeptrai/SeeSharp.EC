using Domain.Utilities.Events.CustomerEvents;
using System;
using Xunit;

namespace SeeSharp.EC.Application.UnitTest.EventsTest;

public class CustomerEventsTests
{
    [Fact]
    public void CustomerChangePasswordEvent_Properties_Should_Be_Set_Correctly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var verificationTokenId = Guid.NewGuid();
        var email = "customer@example.com";
        var messageId = Guid.NewGuid();

        // Act
        var @event = new CustomerChangePasswordEvent(
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
    public void CustomerResetPasswordEvent_Properties_Should_Be_Set_Correctly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var email = "customer@example.com";
        var randomPassword = "RandomPassword123!";
        var messageId = Guid.NewGuid();

        // Act
        var @event = new CustomerResetPasswordEvent(
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
    public void CustomerResetPasswordEmailSendEvent_Properties_Should_Be_Set_Correctly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var verificationTokenId = Guid.NewGuid();
        var email = "customer@example.com";
        var messageId = Guid.NewGuid();

        // Act
        var @event = new CustomerResetPasswordEmailSendEvent(
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
    public void CustomerConfirmChangePasswordEvent_Properties_Should_Be_Set_Correctly()
    {
        // Arrange
        var email = "customer@example.com";
        var messageId = Guid.NewGuid();

        // Act
        var @event = new CustomerConfirmChangePasswordEvent(
            email,
            messageId);

        // Assert
        Assert.Equal(email, @event.Email);
        Assert.Equal(messageId, @event.MessageId);
    }

    [Fact]
    public void AccountVerificationEmailSentEvent_Properties_Should_Be_Set_Correctly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var verificationTokenId = Guid.NewGuid();
        var email = "customer@example.com";
        var messageId = Guid.NewGuid();

        // Act
        var @event = new AccountVerificationEmailSentEvent(
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
    public void CustomerEvents_With_Same_Values_Should_Be_Equal()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var verificationTokenId = Guid.NewGuid();
        var email = "customer@example.com";
        var messageId = Guid.NewGuid();

        // Act
        var event1 = new AccountVerificationEmailSentEvent(userId, verificationTokenId, email, messageId);
        var event2 = new AccountVerificationEmailSentEvent(userId, verificationTokenId, email, messageId);

        // Assert - Records have value equality
        Assert.Equal(event1, event2);
        Assert.True(event1 == event2);
    }

    [Fact]
    public void CustomerEvents_With_Different_Values_Should_Not_Be_Equal()
    {
        // Arrange
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var verificationTokenId = Guid.NewGuid();
        var email = "customer@example.com";
        var messageId = Guid.NewGuid();

        // Act
        var event1 = new CustomerChangePasswordEvent(userId1, verificationTokenId, email, messageId);
        var event2 = new CustomerChangePasswordEvent(userId2, verificationTokenId, email, messageId);

        // Assert - Records with different values should not be equal
        Assert.NotEqual(event1, event2);
        Assert.False(event1 == event2);
    }
} 