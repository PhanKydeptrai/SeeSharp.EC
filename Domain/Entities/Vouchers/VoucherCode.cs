using Domain.Primitives;

namespace Domain.Entities.Vouchers;

public sealed class VoucherCode : ValueObject
{
    private VoucherCode(string value) => Value = value;
    public static VoucherCode NewVoucherName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                "Voucher code is required", nameof(value));
        }

        if(value.Length > MaxLength)
        {
            throw new ArgumentException(
                $"Voucher code must not exceed {MaxLength} characters", nameof(value));
        }

        return new VoucherCode(value);
    }
    public static readonly VoucherCode Empty = new VoucherCode(string.Empty);

    private const int MaxLength = 50;
    public string Value { get; }
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}