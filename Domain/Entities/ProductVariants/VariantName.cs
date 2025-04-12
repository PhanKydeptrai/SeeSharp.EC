using Domain.Primitives;

namespace Domain.Entities.ProductVariants;

public sealed class VariantName : ValueObject
{
    private VariantName(string value)
    {
        Value = value;
    }
    public string Value { get; }
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    //* Factory method
    public static VariantName NewVariantName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                nameof(value), "Variant name cannot be empty.");
        }
        if (value.Length > MaxLength)
        {
            throw new ArgumentException(
                nameof(value), $"Variant name cannot be longer than {MaxLength} characters.");
        }

        return new VariantName(value);
    }
    public static VariantName FromString(string value) => NewVariantName(value);
    public static readonly VariantName Empty = new VariantName(string.Empty);
    private const int MaxLength = 50;
}
