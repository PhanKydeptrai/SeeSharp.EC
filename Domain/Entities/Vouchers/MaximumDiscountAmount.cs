using Domain.Primitives;

namespace Domain.Entities.Vouchers;

public sealed class MaximumDiscountAmount : ValueObject
{
    private MaximumDiscountAmount(decimal value) => Value = value;
    public static MaximumDiscountAmount NewMaxDiscountAmount(decimal value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value), "Maximum discount amount cannot be negative");
        }


        return new MaximumDiscountAmount(value);
    }
    public decimal Value { get; }
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
