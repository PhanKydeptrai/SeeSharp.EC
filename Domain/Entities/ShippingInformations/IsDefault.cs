using Domain.Primitives;

namespace Domain.Entities.ShippingInformations;

public sealed class IsDefault : ValueObject
{
    private IsDefault(bool value) => Value = value;
    public bool Value { get; }
    public static IsDefault FromBoolean(bool value) => value ? True : False;
    public static readonly IsDefault True = new IsDefault(true);
    public static readonly IsDefault False = new IsDefault(false); 

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}