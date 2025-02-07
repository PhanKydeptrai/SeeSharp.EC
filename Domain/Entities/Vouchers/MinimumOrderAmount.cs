using Domain.Primitives;

namespace Domain.Entities.Vouchers;

public sealed class MinimumOrderAmount : ValueObject
{
    private MinimumOrderAmount(decimal value) => Value = value;
    public static MinimumOrderAmount NewMaxDiscountAmount(decimal value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value), "Minimum discount amount cannot be negative");
        }


        return new MinimumOrderAmount(value);
    }

    public static MinimumOrderAmount FromDecimal(decimal value) => new MinimumOrderAmount(value);
    public decimal Value { get; }
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
