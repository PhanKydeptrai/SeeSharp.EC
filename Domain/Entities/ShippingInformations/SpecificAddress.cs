using Domain.Primitives;

namespace Domain.Entities.ShippingInformations;

public sealed class SpecificAddress : ValueObject
{
    private SpecificAddress(string value) => Value = value;
    public string Value { get; }
    public static SpecificAddress NewSpecificAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                "Value cannot be null or whitespace.", nameof(value));
        }
        if (value.Length > MaxLength)
        {
            throw new ArgumentException(
                $"Value cannot be longer than {MaxLength} characters.", nameof(value));
        }
        return new SpecificAddress(value);
    }
    public static readonly SpecificAddress Empty = new SpecificAddress(string.Empty);
    private const int MaxLength = 255; 
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
