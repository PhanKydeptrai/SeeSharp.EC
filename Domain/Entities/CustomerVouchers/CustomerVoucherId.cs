using Domain.Primitives;

namespace Domain.Entities.CustomerVouchers;
public class CustomerVoucherId : ValueObject
{
    private CustomerVoucherId(Ulid value) => Value = value;
    public Ulid Value { get; }
    public static CustomerVoucherId New() => new(Ulid.NewUlid());
    public static CustomerVoucherId FromString(string value) => new(Ulid.Parse(value));
    public static readonly CustomerVoucherId Empty = new(Ulid.Empty);
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}