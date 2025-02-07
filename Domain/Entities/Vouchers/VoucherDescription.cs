using Domain.Primitives;

namespace Domain.Entities.Vouchers;

public sealed class VoucherDescription : ValueObject
{
    public string Value { get; }

    private VoucherDescription(string value) => Value = value;
    public static VoucherDescription NewVoucherDescription(string value) 
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                "Voucher description is required", nameof(value));
        }

        if (value.Length > MaxLength)
        {
            throw new ArgumentException(
                $"Voucher description must have {MaxLength} characters or fewer",
                nameof(value));
        }

        return new VoucherDescription(value);
    } 
    public static VoucherDescription FromString(string value) => new VoucherDescription(value);
    private const int MaxLength = 255;
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}