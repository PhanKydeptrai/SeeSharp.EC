namespace Persistence.Database.PostgreSQL.ReadModels;

public partial class UserAuthenticationTokenReadModel
{
    public string UserAuthenticationTokenId { get; set; } = null!;

    public string Value { get; set; } = null!;

    public string TokenType { get; set; } = null!;

    public DateTime ExpiredTime { get; set; }

    public bool IsBlackList { get; set; }

    public string UserId { get; set; } = null!;

    public UserReadModel User { get; set; } = null!;
}
