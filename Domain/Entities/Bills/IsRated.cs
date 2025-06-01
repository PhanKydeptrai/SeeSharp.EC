using Domain.Primitives;

namespace Domain.Entities.Bills;

public sealed class IsRated : ValueObject
{
    private IsRated(bool value) => Value = value;
    public bool Value { get; }
    public static readonly IsRated Rated = new IsRated(true);
    public static readonly IsRated NotRated = new IsRated(false);
    public static IsRated FromBoolean(bool value) => value ? Rated : NotRated;
    
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
