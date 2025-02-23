namespace Domain.Database.PostgreSQL.ReadModels;

public class VerificationTokenReadModel
{
    public Guid VerificationTokenId { get; set; }
    public string? Temporary { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ExpiredDate { get; set; }
    public UserReadModel UserReadModel { get; set; } = null!;
}
