using Domain.Primitives;

namespace Domain.Entities.Products;

public sealed class ProductPrice : ValueObject
{
    private ProductPrice(decimal value)
    {
        Value = value;
    }
    public static ProductPrice NewProductPrice(decimal value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Price cannot be negative");
        }
        
        return new ProductPrice(value);
    }
    public static ProductPrice FromDecimal(decimal value) => NewProductPrice(value);
    public decimal Value { get; }
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

