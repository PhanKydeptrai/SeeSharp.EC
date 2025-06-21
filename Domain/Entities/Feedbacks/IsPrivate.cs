using Domain.Primitives;

namespace Domain.Entities.Feedbacks;

public class IsPrivate : ValueObject
{
    private IsPrivate(bool value) => Value = value;
    public bool Value { get; }
    public static readonly IsPrivate True = new IsPrivate(true);
    public static readonly IsPrivate False = new IsPrivate(false);
    public static IsPrivate FromBoolean(bool value) => value ? True : False;
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
