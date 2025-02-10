using Domain.Primitives;

namespace Domain.Entities.OrderTransactions;

public sealed class OrderTransactionId : ValueObject
{
    private OrderTransactionId(Ulid value) => Value = value;
    public Ulid Value { get; }
    public static OrderTransactionId New() => new(Ulid.NewUlid());
    public static OrderTransactionId FromString(string value) => new(Ulid.Parse(value));
    public static readonly OrderTransactionId Empty = new(Ulid.Empty);
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
