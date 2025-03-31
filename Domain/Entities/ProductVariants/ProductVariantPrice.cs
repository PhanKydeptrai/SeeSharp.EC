using Domain.Primitives;

namespace Domain.Entities.ProductVariants;

public sealed class ProductVariantPrice : ValueObject
{
    private ProductVariantPrice(decimal value)
    {
        Value = value;
    }
    public static ProductVariantPrice NewProductPrice(decimal value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Price cannot be negative");
        }
        
        return new ProductVariantPrice(value);
    }
    public static ProductVariantPrice FromDecimal(decimal value) => NewProductPrice(value);
    public decimal Value { get; }
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

