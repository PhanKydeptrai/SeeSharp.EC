using Domain.Primitives;

namespace Domain.Entities.OrderDetails;

public sealed class OrderDetailUnitPrice : ValueObject
{
    public OrderDetailUnitPrice(decimal value) => Value = value;
    public decimal Value { get; }
    public static OrderDetailUnitPrice NewOrderDetailUnitPrice(decimal value) 
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value), "OrderDetailUnitPrice cannot be less than or equal to zero.");
        }
        return new OrderDetailUnitPrice(value);
    }
    public static OrderDetailUnitPrice FromDecimal(decimal value) => new OrderDetailUnitPrice(value);

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
