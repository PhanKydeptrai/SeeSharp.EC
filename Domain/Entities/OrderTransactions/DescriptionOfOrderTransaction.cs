using Domain.Primitives;

namespace Domain.Entities.OrderTransactions;

public sealed class DescriptionOfOrderTransaction : ValueObject
{
    private DescriptionOfOrderTransaction(string value) => Value = value;
    public static DescriptionOfOrderTransaction NewDescriptionOfOrderTransaction(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(
                nameof(value), "Description must not be empty");
        }
        if (value.Length > MaxLength)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value), $"Description must not exceed {MaxLength} characters");
        }

        return new DescriptionOfOrderTransaction(value);
    }

    public static DescriptionOfOrderTransaction FromString(string value) => new DescriptionOfOrderTransaction(value);
    public string Value { get; }
    private const int MaxLength = 255;
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}