using Domain.Primitives;

namespace Domain.Entities.Orders;

public sealed class OrderTotal : ValueObject
{
    public OrderTotal(decimal value) => Value = value;
    public decimal Value { get; }
    public static OrderTotal NewOrderTotal(decimal value) 
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value), "Order total cannot be negative.");
        }
        return new OrderTotal(value);
    } 
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

