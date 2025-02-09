namespace Persistence.Database.PostgreSQL.ReadModels;

public class VerificationTokenReadModel
{
    public Ulid VerificationTokenId { get; set; }
    public string? Temporary { get; set; }
    public Ulid UserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ExpiredDate { get; set; }
    public UserReadModel UserReadModel { get; set; } = null!;
}
