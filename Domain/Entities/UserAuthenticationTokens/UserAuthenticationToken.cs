using Domain.Entities.Users;
using Domain.Entities.VerificationTokens;

namespace Domain.Entities.UserAuthenticationTokens;
public sealed class UserAuthenticationToken
{
    public UserAuthenticationTokenId UserAuthenticationTokenId { get; private set; } = null!;
    public string Value { get; private set; } = string.Empty;
    public string Jti { get; private set; } = string.Empty;
    public DateTime ExpiredTime { get; private set; }
    public IsBlackList IsBlackList { get; private set; } = null!;
    public UserId UserId { get; private set; } = null!;
    public ChainId ChainId { get; private set; } = null!;
    public User? User { get; set; } = null!;

    private UserAuthenticationToken(
        UserAuthenticationTokenId userAuthenticationTokenId,
        string value,
        string jti,
        DateTime expiredTime,
        IsBlackList isBlackList,
        ChainId chainId,
        UserId userId)
    {
        UserAuthenticationTokenId = userAuthenticationTokenId;
        Value = value;
        Jti = jti;
        ExpiredTime = expiredTime;
        IsBlackList = isBlackList;
        ChainId = chainId;
        UserId = userId;
    }

    //Factory Method
    public static UserAuthenticationToken NewUserAuthenticationToken(
        string value,
        string jti,
        DateTime expiredTime,
        ChainId? chainId, // can be null, so we can reuse ChainId for multiple token
        UserId userId)
    {
        if (expiredTime <= DateTime.UtcNow)
        {
            throw new ArgumentException(
                "Expired time must be greater than current time",
                nameof(expiredTime));
        }

        return new UserAuthenticationToken(
            UserAuthenticationTokenId.New(),
            value,
            jti,
            expiredTime,
            IsBlackList.False,
            chainId ?? ChainId.New(), 
            userId);
    }

    public void BlackList()
    {
        IsBlackList = IsBlackList.True;
    }
}
