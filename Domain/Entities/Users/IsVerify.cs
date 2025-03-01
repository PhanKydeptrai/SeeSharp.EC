using Domain.Primitives;

namespace Domain.Entities.Users;

public sealed class IsVerify : ValueObject
{
    private IsVerify(bool value) => Value = value;
    public static readonly IsVerify True = new IsVerify(true);
    public static readonly IsVerify False = new IsVerify(false);
    public static IsVerify FromBoolean(bool value) => value ? True : False;
    public bool Value { get; }
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
