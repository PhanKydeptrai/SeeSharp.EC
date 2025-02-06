using Domain.Primitives;

namespace Domain.Entities.ShippingInformations;

public sealed class Ward : ValueObject
{
    private Ward(string value) => Value = value;
    public string Value { get; }
    public static Ward NewDistrict(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                "Ward is required", nameof(value));
        }
        if (value.Length > MaxLength)
        {
            throw new ArgumentException(
                $"Ward must be less than {MaxLength}", nameof(value));
        }

        return new Ward(value);
    }
    public static readonly int MaxLength = 50;
    public static readonly Ward Empty = new Ward(string.Empty);
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}