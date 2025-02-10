using Domain.Primitives;

namespace Domain.Entities.UserAuthenticationTokens;

public sealed class UserAuthenticationTokenId : ValueObject
{
    private UserAuthenticationTokenId(Ulid value) => Value = value;
    public Ulid Value { get; }
    public static UserAuthenticationTokenId New() => new(Ulid.NewUlid());
    public static UserAuthenticationTokenId FromString(string value) => new(Ulid.Parse(value));
    public static readonly UserAuthenticationTokenId Empty = new(Ulid.Empty);
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
