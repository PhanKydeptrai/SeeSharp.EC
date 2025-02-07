using Domain.Primitives;

namespace Domain.Entities.OrderDetails;

public sealed class OrderDetailQuantity : ValueObject
{
    private OrderDetailQuantity(int value) => Value = value;
    public int Value { get; }
    public static OrderDetailQuantity NewOrderDetailQuantity(int value) 
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value), "OrderDetailQuantity must be greater than zero.");
        }
        return new OrderDetailQuantity(value);   
    }
    public static OrderDetailQuantity FromInt(int value) => new OrderDetailQuantity(value);
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
