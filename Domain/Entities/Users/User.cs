namespace Domain.Entities.Users;

public sealed class User
{
    public UserId UserId { get; private set; } = null!;
    public UserName UserName { get; private set; } = null!;
    public Email? Email { get; private set; } = Email.Empty;
    public PhoneNumber? PhoneNumber { get; private set; } = PhoneNumber.Empty;
    public PasswordHash? PasswordHash { get; private set; } = PasswordHash.Empty;
    public UserStatus UserStatus { get; private set; }
    public bool IsVerify { get; private set; }
    public Gender Gender { get; private set; }
    public DateTime? DateOfBirth { get; private set; }
    public string? ImageUrl { get; private set; } = string.Empty;
    //public Customer? Customer { get; set; } = null!;
    //public Employee? Employee { get; set; } = null!;
    //public ICollection<UserAuthenticationToken>? UserAuthenticationTokens { get; set; } = null!;
    //public ICollection<VerificationToken>? VerificationTokens { get; set; } = null!;
    private User(Ulid userId,
        string userName,
        string email,
        string phoneNumber,
        string passwordHash,
        UserStatus userStatus,
        bool isVerify,
        Gender gender,
        DateTime? dateOfBirth,
        string? imageUrl)
    {
        UserId = new UserId(userId);
        UserName = UserName.NewUserName(userName);
        Email = Email.NewEmail(email) ?? Email.Empty;
        PhoneNumber = PhoneNumber.NewPhoneNumber(phoneNumber) ?? PhoneNumber.Empty;
        PasswordHash = PasswordHash.NewPasswordHash(passwordHash) ?? PasswordHash.Empty;
        UserStatus = userStatus;
        Gender = gender;
        IsVerify = isVerify;
        DateOfBirth = dateOfBirth;
        ImageUrl = imageUrl ?? string.Empty;
    }
    //Factory Method
    public User NewUser(Ulid userId,
        string userName,
        string email,
        string? phoneNumber,
        string? passwordHash,
        DateTime? dateOfBirth,
        string? imageUrl)
    {
        return new User(
            userId, 
            userName, 
            email ?? string.Empty, 
            phoneNumber ?? string.Empty, 
            passwordHash ?? string.Empty, 
            UserStatus.Inactive, 
            false, 
            Gender.Unknown,
            dateOfBirth, 
            imageUrl ?? string.Empty);
    }
}
