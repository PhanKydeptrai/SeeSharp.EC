namespace Persistence.Database.PostgreSQL.ReadModels;

public class UserReadModel
{
    public string UserId { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string UserStatus { get; set; } = null!;

    public bool IsVerify { get; set; }

    public string Gender { get; set; } = null!;

    public DateTime? DateOfBirth { get; set; }

    public string? ImageUrl { get; set; }

    public virtual CustomerReadModel? Customer { get; set; }

    public virtual EmployeeReadModel? Employee { get; set; }

    public virtual ICollection<UserAuthenticationToken> UserAuthenticationTokens { get; set; } = new List<UserAuthenticationToken>();

    public virtual ICollection<VerifyVerificationTokenReadModel> VerifyVerificationTokens { get; set; } = new List<VerifyVerificationTokenReadModel>();
}
