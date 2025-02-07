using Domain.Primitives;

namespace Domain.Entities.UserAuthenticationTokens;

public sealed class IsBlackList : ValueObject
{
    public IsBlackList(bool value) => Value = value;
    public static readonly IsBlackList False = new IsBlackList(false);
    public static readonly IsBlackList True = new IsBlackList(true);
    public static IsBlackList FromBoolean(bool value) => value ? True : False;
    public bool Value { get; }
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}