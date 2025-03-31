using Domain.Primitives;

namespace Domain.Entities.ProductVariants;

public sealed class ProductVariantDescription : ValueObject
{
    private ProductVariantDescription(string value)
    {
        Value = value;
    }
    public string Value { get; }
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    //* Factory method
    public static ProductVariantDescription Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Product variant description cannot be empty.", 
                nameof(value));
        }

        if (value.Length > MaxLength)
        {
            throw new ArgumentException($"Product variant description must be {MaxLength} characters or fewer.", 
                nameof(value));
        }

        return new ProductVariantDescription(value);
    }

    public static ProductVariantDescription FromString(string value)
    {
        return new ProductVariantDescription(value);
    }

    public static ProductVariantDescription Empty => new(string.Empty);

    private const int MaxLength = 255;
}
