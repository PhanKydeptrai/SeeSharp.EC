using Domain.Primitives;

namespace Domain.Entities.ShippingInformations;

public sealed class FullName : ValueObject
{
    public string Value { get; }
    private FullName(string value) => Value = value;
    public static FullName NewFullName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(
                nameof(value), "Full name cannot be empty");
        }

        if(value.Length > MaxLength)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value), $"Full name cannot be longer than {MaxLength} characters");
        }


        return new FullName(value);
    }
    public static FullName FromString(string value) => new FullName(value);

    public static readonly FullName Empty = new FullName(string.Empty);
    private const int MaxLength = 50;
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
