namespace Domain.ReadModels;

public class VerificationTokenReadModel
{
    public Ulid VerificationTokenId { get; set; } = Ulid.NewUlid();

    public string? Temporary { get; set; } = string.Empty;

    public Ulid UserId { get; set; } = Ulid.NewUlid();

    public DateTime CreatedDate { get; set; }

    public DateTime ExpiredDate { get; set; }
}
