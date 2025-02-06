using Domain.Primitives;

namespace Domain.Entities.Products;

public sealed class ProductName : ValueObject
{
    private ProductName(string value)
    {
        Value = value;
    }
    public string Value { get; }
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    //* Factory method
    public static ProductName NewProductName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(
                nameof(value), "Product name cannot be empty.");
        }
        if (value.Length > MaxLength)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value), $"Product name cannot be longer than {MaxLength} characters.");
        }

        return new ProductName(value);
    }

    public static readonly ProductName Empty = new ProductName(string.Empty); 

    private const int MaxLength = 50;
}

