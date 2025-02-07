using Domain.Primitives;

namespace Domain.Entities.ShippingInformations;

public sealed class District : ValueObject
{
    private District(string value) => Value = value;
    public string Value { get; }
    public static District NewDistrict(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                "District is required", nameof(value));
        }
        if (value.Length > MaxLength)
        {
            throw new ArgumentException(
                $"District must be less than {MaxLength}", nameof(value));
        }

        return new District(value);
    }
    public static District FromString(string value) => new District(value);
    public static readonly District Empty = new District(string.Empty);
    private const int MaxLength = 50;
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
