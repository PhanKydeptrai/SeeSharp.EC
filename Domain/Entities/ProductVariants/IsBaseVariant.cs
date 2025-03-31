using Domain.Primitives;

namespace Domain.Entities.ProductVariants;

public sealed class IsBaseVariant : ValueObject
{
    private IsBaseVariant(bool value)
    {
        Value = value;
    }
    public bool Value { get; }
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    //* Factory method
    public static readonly IsBaseVariant True = new IsBaseVariant(true);
    public static readonly IsBaseVariant False = new IsBaseVariant(false);
    public static IsBaseVariant FromBoolean(bool value) => value ? True : False;
}
