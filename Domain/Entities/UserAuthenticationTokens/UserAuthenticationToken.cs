using Domain.Entities.Users;

namespace Domain.Entities.UserAuthenticationTokens;
//NOTE: Create factory method
public sealed class UserAuthenticationToken
{
    public UserAuthenticationTokenId UserAuthenticationTokenId { get; private set; } = null!;
    public string Value { get; private set; } = null!;
    public TokenType TokenType { get; private set; } 
    public DateTime ExpiredTime { get; private set; }
    public IsBlackList IsBlackList { get; private set; } = null!;
    public UserId UserId { get; private set; } = null!;
    public User? User { get; set; } = null!;
}
