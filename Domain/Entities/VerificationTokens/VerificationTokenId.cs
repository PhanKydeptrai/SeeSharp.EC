using Domain.Primitives;

namespace Domain.Entities.VerificationTokens;

public sealed class VerificationTokenId : ValueObject
{
    private VerificationTokenId(Ulid value) => Value = value;
    public Ulid Value { get; }
    public static VerificationTokenId New() => new(Ulid.NewUlid());
    public static VerificationTokenId FromString(string value) => new(Ulid.Parse(value));
    public static readonly VerificationTokenId Empty = new(Ulid.Empty);
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}