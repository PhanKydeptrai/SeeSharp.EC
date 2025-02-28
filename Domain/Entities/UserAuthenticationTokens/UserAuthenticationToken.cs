using Domain.Entities.Users;

namespace Domain.Entities.UserAuthenticationTokens;
public sealed class UserAuthenticationToken
{
    public UserAuthenticationTokenId UserAuthenticationTokenId { get; private set; } = null!;
    public string Value { get; private set; } = null!;
    public TokenType TokenType { get; private set; } 
    public DateTime ExpiredTime { get; private set; }
    public IsBlackList IsBlackList { get; private set; } = null!;
    public UserId UserId { get; private set; } = null!;
    public User? User { get; set; } = null!;

    private UserAuthenticationToken(
        UserAuthenticationTokenId userAuthenticationTokenId,
        string value,
        TokenType tokenType,
        DateTime expiredTime,
        IsBlackList isBlackList,
        UserId userId)
    {
        UserAuthenticationTokenId = userAuthenticationTokenId;
        Value = value;
        TokenType = tokenType;
        ExpiredTime = expiredTime;
        IsBlackList = isBlackList;
        UserId = userId;
    }

    //Factory Method
    public static UserAuthenticationToken NewUserAuthenticationToken(
        string value,
        TokenType tokenType,
        DateTime expiredTime,
        UserId userId)
    {
        if(expiredTime <= DateTime.UtcNow)
        {
            throw new ArgumentException(
                "Expired time must be greater than current time", 
                nameof(expiredTime));
        }
        return new UserAuthenticationToken(
            UserAuthenticationTokenId.New(),
            value,
            tokenType,
            expiredTime,
            IsBlackList.False,
            userId);
    }
}
