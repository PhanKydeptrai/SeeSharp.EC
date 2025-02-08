namespace Persistence.Database.PostgreSQL.ReadModels;

public class VerifyVerificationTokenReadModel
{
    public string VerificationTokenId { get; set; } = null!;

    public string? Temporary { get; set; }

    public string UserId { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime ExpiredDate { get; set; }

    public virtual UserReadModel User { get; set; } = null!;
}
