using Domain.Entities.Users;

namespace Domain.Entities.VerificationTokens;
//NOTE: Create factory method
public sealed class VerificationToken
{
    public VerificationTokenId VerificationTokenId { get; private set; } = null!;
    public string? Temporary { get; private set; } = string.Empty;
    public UserId UserId { get; private set; } = null!;
    public DateTime CreatedDate { get; private set; }
    public DateTime ExpiredDate { get; private set; }
    public User? User { get; set; } = null!;
}
