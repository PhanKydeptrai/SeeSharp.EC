namespace Persistence.Database.PostgreSQL.ReadModels;

public partial class UserAuthenticationTokenReadModel
{
    public Ulid UserAuthenticationTokenId { get; set; } 
    public string Value { get; set; } = null!;
    public string TokenType { get; set; } = null!;
    public DateTime ExpiredTime { get; set; }
    public bool IsBlackList { get; set; }
    public Ulid UserId { get; set; }
    public UserReadModel UserReadModel { get; set; } = null!;
}
