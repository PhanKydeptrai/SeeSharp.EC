using Domain.Primitives;

namespace Domain.Entities.Vouchers;

public sealed class VoucherName : ValueObject
{
    private VoucherName(string value) => Value = value;
    public static VoucherName NewVoucherName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                "Voucher name is required", nameof(value));
        }

        if(value.Length > MaxLength)
        {
            throw new ArgumentException(
                $"Voucher name must not exceed {MaxLength} characters", nameof(value));
        }

        return new VoucherName(value);
    }
    public static readonly VoucherName Empty = new VoucherName(string.Empty);

    private const int MaxLength = 50;
    public string Value { get; }
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
