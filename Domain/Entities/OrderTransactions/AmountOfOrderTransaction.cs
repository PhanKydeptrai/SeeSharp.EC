using Domain.Primitives;

namespace Domain.Entities.OrderTransactions;

public sealed class AmountOfOrderTransaction : ValueObject
{
    private AmountOfOrderTransaction(decimal value) => Value = value;
    public static AmountOfOrderTransaction NewAmountOfOrderTransaction(decimal value)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value), "Amount must be greater than 0");
        }
        return new AmountOfOrderTransaction(value);
    }
    public decimal Value { get; }
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
