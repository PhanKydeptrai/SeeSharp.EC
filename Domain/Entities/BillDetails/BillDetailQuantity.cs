using Domain.Primitives;

namespace Domain.Entities.BillDetails;

public sealed class BillDetailQuantity : ValueObject
{
    private BillDetailQuantity(int value) => Value = value;
    public int Value { get; }
    public static BillDetailQuantity NewOrderDetailQuantity(int value)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value), "BillDetailQuantity must be greater than zero.");
        }
        return new BillDetailQuantity(value);
    }

    public static BillDetailQuantity FromInt(int value) => new BillDetailQuantity(value);
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
