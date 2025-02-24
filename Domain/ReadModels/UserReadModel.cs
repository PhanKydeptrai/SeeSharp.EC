namespace Domain.Database.PostgreSQL.ReadModels;

public sealed class UserReadModel
{
    public Ulid UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string UserStatus { get; set; } = null!;
    public bool IsVerify { get; set; }
    public string Gender { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }
    public string? ImageUrl { get; set; }
    public CustomerReadModel? CustomerReadModel { get; set; }
    public EmployeeReadModel? EmployeeReadModel { get; set; }
    public ICollection<UserAuthenticationTokenReadModel> UserAuthenticationTokenReadModels { get; set; } = new List<UserAuthenticationTokenReadModel>();
    public ICollection<VerificationTokenReadModel> VerificationTokenReadModels { get; set; } = new List<VerificationTokenReadModel>();
}
