using Domain.Database.PostgreSQL.ReadModels;
using Domain.Entities.Users;
using Domain.ReadModels;
using System;
using System.Collections.Generic;
using Xunit;

namespace SeeSharp.EC.Application.UnitTest.ReadModels;

public class UserReadModelTests
{
    [Fact]
    public void UserReadModel_Properties_Should_Be_Set_Correctly()
    {
        // Arrange
        var userId = Ulid.NewUlid();
        var userName = "testuser";
        var email = "test@example.com";
        var phoneNumber = "0123456789";
        var passwordHash = "hashedpassword";
        var userStatus = UserStatus.Active;
        var isVerify = true;
        var gender = 1;
        var dateOfBirth = new DateTime(1990, 1, 1);
        var imageUrl = "http://example.com/image.jpg";

        // Act
        var userReadModel = new UserReadModel
        {
            UserId = userId,
            UserName = userName,
            Email = email,
            PhoneNumber = phoneNumber,
            PasswordHash = passwordHash,
            UserStatus = userStatus,
            IsVerify = isVerify,
            Gender = gender,
            DateOfBirth = dateOfBirth,
            ImageUrl = imageUrl,
            VerificationTokenReadModels = new List<VerificationTokenReadModel>()
        };

        // Assert
        Assert.Equal(userId, userReadModel.UserId);
        Assert.Equal(userName, userReadModel.UserName);
        Assert.Equal(email, userReadModel.Email);
        Assert.Equal(phoneNumber, userReadModel.PhoneNumber);
        Assert.Equal(passwordHash, userReadModel.PasswordHash);
        Assert.Equal(userStatus, userReadModel.UserStatus);
        Assert.Equal(isVerify, userReadModel.IsVerify);
        Assert.Equal(gender, userReadModel.Gender);
        Assert.Equal(dateOfBirth, userReadModel.DateOfBirth);
        Assert.Equal(imageUrl, userReadModel.ImageUrl);
        Assert.NotNull(userReadModel.VerificationTokenReadModels);
        Assert.Empty(userReadModel.VerificationTokenReadModels);
    }

    [Fact]
    public void UserReadModel_Can_Have_Related_Entities()
    {
        // Arrange
        var userId = Ulid.NewUlid();
        var customerReadModel = new CustomerReadModel { CustomerId = Ulid.NewUlid() };
        var employeeReadModel = new EmployeeReadModel { EmployeeId = Ulid.NewUlid() };
        var verificationToken = new VerificationTokenReadModel { VerificationTokenId = Ulid.NewUlid() };

        // Act
        var userReadModel = new UserReadModel
        {
            UserId = userId,
            UserName = "testuser",
            Email = "test@example.com",
            PhoneNumber = "0123456789",
            PasswordHash = "hashedpassword",
            CustomerReadModel = customerReadModel,
            EmployeeReadModel = employeeReadModel,
            VerificationTokenReadModels = new List<VerificationTokenReadModel> { verificationToken }
        };

        // Assert
        Assert.NotNull(userReadModel.CustomerReadModel);
        Assert.Equal(customerReadModel.CustomerId, userReadModel.CustomerReadModel.CustomerId);
        
        Assert.NotNull(userReadModel.EmployeeReadModel);
        Assert.Equal(employeeReadModel.EmployeeId, userReadModel.EmployeeReadModel.EmployeeId);
        
        Assert.NotNull(userReadModel.VerificationTokenReadModels);
        Assert.Single(userReadModel.VerificationTokenReadModels);
        Assert.Equal(verificationToken.VerificationTokenId, userReadModel.VerificationTokenReadModels.First().VerificationTokenId);
    }

    [Fact]
    public void UserReadModel_Optional_Properties_Can_Be_Null()
    {
        // Arrange & Act
        var userReadModel = new UserReadModel
        {
            UserId = Ulid.NewUlid(),
            UserName = "testuser",
            Email = "test@example.com",
            PhoneNumber = "0123456789",
            PasswordHash = "hashedpassword",
            UserStatus = UserStatus.Active,
            IsVerify = true,
            Gender = 1,
            // Optional properties not set
            DateOfBirth = null,
            ImageUrl = null,
            CustomerReadModel = null,
            EmployeeReadModel = null
        };

        // Assert
        Assert.Null(userReadModel.DateOfBirth);
        Assert.Null(userReadModel.ImageUrl);
        Assert.Null(userReadModel.CustomerReadModel);
        Assert.Null(userReadModel.EmployeeReadModel);
        Assert.NotNull(userReadModel.VerificationTokenReadModels);
        Assert.Empty(userReadModel.VerificationTokenReadModels);
    }
} 