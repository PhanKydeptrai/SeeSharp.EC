using Domain.Primitives;

namespace Domain.Entities.OrderDetails;

// public record OrderDetailId(Ulid Value);
public class OrderDetailId : ValueObject
{
    private OrderDetailId(Ulid value) => Value = value;
    public Ulid Value { get; }
    public static OrderDetailId New() => new(Ulid.NewUlid());
    public static OrderDetailId FromString(string value) => new(Ulid.Parse(value));
    public static readonly OrderDetailId Empty = new(Ulid.Empty);
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
