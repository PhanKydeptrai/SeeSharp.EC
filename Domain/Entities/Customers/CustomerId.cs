using Domain.Primitives;

namespace Domain.Entities.Customers;

public class CustomerId : ValueObject
{
    private CustomerId(Ulid value) => Value = value;
    public Ulid Value { get; }
    public static CustomerId New() => new(Ulid.NewUlid());
    public static CustomerId FromString(string value) => new(Ulid.Parse(value));
    public static readonly CustomerId Empty = new(Ulid.Empty);
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

