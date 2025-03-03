using System.Text.RegularExpressions;
using Domain.Primitives;

namespace Domain.Entities.Users;

public sealed class Email : ValueObject
{
    private Email(string value)
    {
        Value = value;
    }

    public static Email NewEmail(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(
                nameof(value), "Email cannot be empty");
        }

        if (value.Length > MaxLength)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value), $"Email cannot be longer than {MaxLength} characters");
        }

        if (!Regex.IsMatch(value, EmailRegex))
        {
            throw new ArgumentException(
                nameof(value), "Email is invalid");
        }

        return new Email(value);
    }
    public static Email FromString(string value) => new Email(value);
    public override string ToString() => Value;
    public readonly static Email Empty = new Email(string.Empty);

    private const string EmailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    private const int MaxLength = 50;
    public string Value { get; }
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
