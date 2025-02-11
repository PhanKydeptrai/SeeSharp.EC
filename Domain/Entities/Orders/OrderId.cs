using Domain.Primitives;

namespace Domain.Entities.Orders;
public sealed class OrderId : ValueObject
{
    private OrderId(Ulid value) => Value = value;
    public Ulid Value { get; }
    public static OrderId New() => new(Ulid.NewUlid());
    public static OrderId FromString(string value) => new(Ulid.Parse(value));
    public static readonly OrderId Empty = new(Ulid.Empty);
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

