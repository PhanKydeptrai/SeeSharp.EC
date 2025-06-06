﻿using Domain.Entities.Customers;
using Domain.Entities.Employees;
using Domain.Entities.UserAuthenticationTokens;
using Domain.Entities.VerificationTokens;

namespace Domain.Entities.Users;

public sealed class User
{
    public UserId UserId { get; private set; } = null!;
    public UserName UserName { get; private set; } = null!;
    public Email? Email { get; private set; } = Email.Empty;
    public PhoneNumber? PhoneNumber { get; private set; } = PhoneNumber.Empty;
    public PasswordHash? PasswordHash { get; private set; } = PasswordHash.Empty;
    public UserStatus UserStatus { get; private set; }
    public IsVerify IsVerify { get; private set; } 
    public Gender Gender { get; private set; }
    public DateOnly? DateOfBirth { get; private set; }
    public string? ImageUrl { get; set; }
    public Customer? Customer { get; set; } = null!;
    public Employee? Employee { get; set; } = null!;
    public ICollection<UserAuthenticationToken>? UserAuthenticationTokens { get; set; } = null!;
    public ICollection<VerificationToken>? VerificationTokens { get; set; } = null!;
    private User(UserId userId,
        UserName userName,
        Email email,
        PhoneNumber phoneNumber,
        PasswordHash passwordHash,
        UserStatus userStatus,
        IsVerify isVerify,
        Gender gender,
        DateOnly? dateOfBirth,
        string? imageUrl)
    {
        UserId = userId;
        UserName = userName;
        Email = email;
        PhoneNumber = phoneNumber;
        PasswordHash = passwordHash;
        UserStatus = userStatus;
        Gender = gender;
        IsVerify = isVerify;
        DateOfBirth = dateOfBirth;
        ImageUrl = imageUrl;
    }

    //Factory Method
    public static User NewUser(
        UserId? userId,
        UserName userName,
        Email email,
        PhoneNumber? phoneNumber,
        PasswordHash? passwordHash,
        DateOnly? dateOfBirth,
        string? imageUrl)
    {
        return new User(
            userId ?? UserId.New(), 
            userName, 
            email, 
            phoneNumber ?? PhoneNumber.Empty, 
            passwordHash ?? PasswordHash.Empty, 
            UserStatus.InActive,
            IsVerify.False, 
            Gender.Unknown,
            dateOfBirth, 
            imageUrl ?? string.Empty);
    }

    public void UpdateUser(
        UserName userName,
        PhoneNumber? phoneNumber, 
        DateOnly? dateOfBirth,
        Gender gender,
        string? imageUrl)
    {
        UserName = userName;
        PhoneNumber = phoneNumber;
        DateOfBirth = dateOfBirth;
        Gender = gender;
        ImageUrl = imageUrl;
    }

    public void ChangePassword(PasswordHash passwordHash)
    {
        PasswordHash = passwordHash;
    }

    public void VerifyAccount()
    {
        if (IsVerify == IsVerify.True)
        {
            throw new InvalidOperationException("User is already verified");
        }

        if (UserStatus == UserStatus.Deleted)
        {
            throw new InvalidOperationException("User is deleted");
        }

        if (UserStatus == UserStatus.Blocked)
        {
            throw new InvalidOperationException("User is blocked");
        }
        
        IsVerify = IsVerify.True;
        UserStatus = UserStatus.Active;
    }

    

    public void BlockAccount()
    {
        if (UserStatus == UserStatus.Deleted)
        {
            throw new InvalidOperationException("User is deleted");
        }

        if (UserStatus == UserStatus.Blocked)
        {
            throw new InvalidOperationException("User is already blocked");
        }

        UserStatus = UserStatus.Blocked;
    }

    public static User FromExisting(
        UserId userId, 
        UserName userName, 
        Email email, 
        PhoneNumber? phoneNumber, 
        PasswordHash? passwordHash, 
        string imageUrl)
    {
        return new User(
            userId, 
            userName, 
            email, 
            phoneNumber ?? PhoneNumber.Empty, 
            passwordHash ?? PasswordHash.Empty, 
            UserStatus.Active,
            IsVerify.True, 
            Gender.Unknown,
            null, 
            imageUrl ?? string.Empty);
    }

    public void UpdateStatus(UserStatus status)
    {
        UserStatus = status;
    }
}
