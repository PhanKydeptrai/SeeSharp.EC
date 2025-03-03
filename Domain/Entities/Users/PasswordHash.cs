using Domain.Primitives;

namespace Domain.Entities.Users;

public sealed class PasswordHash : ValueObject
{
    private PasswordHash(string value)
    {
        Value = value;
    }
    public static PasswordHash NewPasswordHash(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value), "Password hash cannot be empty");
        }

        if (value.Length != Length)
        {
            throw new ArgumentOutOfRangeException(nameof(value), $"Password hash must be {Length} char");
        }
        return new PasswordHash(value);
    }
    public static PasswordHash FromString(string value) => new PasswordHash(value);
    public readonly static PasswordHash Empty = new PasswordHash(string.Empty);
    public string Value { get; }
    private const int Length = 64;
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
