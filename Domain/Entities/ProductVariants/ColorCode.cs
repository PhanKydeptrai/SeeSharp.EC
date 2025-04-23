using Domain.Primitives;

namespace Domain.Entities.ProductVariants;

public sealed class ColorCode : ValueObject
{
    private ColorCode(string value)
    {
        Value = value;
    }
    public string Value { get; }
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    //* Factory method
    public static ColorCode Create(string value)
    {
        // if (string.IsNullOrWhiteSpace(value))
        // {
        //     throw new ArgumentException("Color code cannot be empty.", nameof(value));
        // }

        // if (value.Length != 7 || !value.StartsWith("#"))
        // {
        //     throw new ArgumentException("Color code must be in the format #RRGGBB.", nameof(value));
        // }
        
        return new ColorCode(value);
    }

    public static ColorCode FromString(string value) => Create(value);
}
