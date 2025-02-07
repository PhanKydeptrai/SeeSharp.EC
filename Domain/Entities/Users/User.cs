using Domain.Entities.Customers;
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
    public bool IsVerify { get; private set; } //FIXME: Change to value object
    public Gender Gender { get; private set; }
    public DateTime? DateOfBirth { get; private set; }
    public string? ImageUrl { get; private set; } = string.Empty;
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
        bool isVerify,
        Gender gender,
        DateTime? dateOfBirth,
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
    public User NewUser(UserId userId,
        UserName userName,
        Email email,
        PhoneNumber? phoneNumber,
        PasswordHash? passwordHash,
        DateTime? dateOfBirth,
        string? imageUrl)
    {
        return new User(
            userId, 
            userName, 
            email, 
            phoneNumber ?? PhoneNumber.Empty, 
            passwordHash ?? PasswordHash.Empty, 
            UserStatus.Inactive, 
            false, 
            Gender.Unknown,
            dateOfBirth, 
            imageUrl ?? string.Empty);
    }
}
