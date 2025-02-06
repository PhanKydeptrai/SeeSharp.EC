using Domain.Primitives;

namespace Domain.Entities.ShippingInformations;

public sealed class Province : ValueObject
{
    private Province(string value) => Value = value;
    public static Province NewProvince(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                "Province is required", nameof(value));
        }
        if (value.Length > MaxLength)
        {
            throw new ArgumentException(
                $"Province must be less than {MaxLength}", nameof(value));
        }

        return new Province(value);
    }
    public static readonly Province Empty = new Province(string.Empty);
    private const int MaxLength = 50;
    public string Value { get; }
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}