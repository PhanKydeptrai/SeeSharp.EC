using Domain.Primitives;

namespace Domain.Entities.Vouchers;

public sealed class PercentageDiscount : ValueObject
{
    private PercentageDiscount(int value) => Value = value;

    public int Value { get; }
    public static PercentageDiscount Create(int value)
    {
        if (value < 0 || value > 100)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value), "Percentage discount must be between 0 and 100");
        }

        return new PercentageDiscount(value);
    }
    public static PercentageDiscount FromDecimal(int value) => new PercentageDiscount(value);
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

