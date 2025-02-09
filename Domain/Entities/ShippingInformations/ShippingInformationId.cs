using Domain.Primitives;

namespace Domain.Entities.ShippingInformations;

public class ShippingInformationId : ValueObject
{
    private ShippingInformationId(Ulid value) => Value = value;
    public Ulid Value { get; }
    public static ShippingInformationId New() => new(Ulid.NewUlid());
    public static ShippingInformationId FromString(string value) => new(Ulid.Parse(value));
    public static readonly ShippingInformationId Empty = new(Ulid.Empty);
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
