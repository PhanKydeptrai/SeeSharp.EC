using Domain.Primitives;

namespace Domain.Entities.Vouchers;

public sealed class VoucherId : ValueObject
{
    private VoucherId(Ulid value) => Value = value;
    public Ulid Value { get; }
    public static VoucherId New() => new(Ulid.NewUlid());
    public static VoucherId FromString(string value) => new(Ulid.Parse(value));
    public static readonly VoucherId Empty = new(Ulid.Empty);
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
