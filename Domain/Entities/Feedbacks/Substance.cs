using Domain.Primitives;

namespace Domain.Entities.Feedbacks;

public sealed class Substance : ValueObject
{
    private Substance(string value) => Value = value;
    public string Value { get; }
    public static Substance NewSubstance(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(
                nameof(value), "Substance must not be empty");
        }
        if (value.Length > MaxLength)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value), $"Substance must not exceed {MaxLength} characters");
        }

        return new Substance(value);
    }
    private const int MaxLength = 50;
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
