using Domain.Primitives;

namespace Domain.Entities.Products;

public sealed class ProductId : ValueObject
{
    private ProductId(Ulid value) => Value = value;
    public Ulid Value { get; }
    public static ProductId New() => new(Ulid.NewUlid());
    public static ProductId FromString(string value) => new(Ulid.Parse(value));
    public static readonly ProductId Empty = new(Ulid.Empty);
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

