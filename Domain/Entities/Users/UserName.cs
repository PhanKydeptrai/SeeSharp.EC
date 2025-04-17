using Domain.Primitives;

namespace Domain.Entities.Users;

public sealed class UserName : ValueObject
{
    private UserName(string value)
    {
        Value = value;
    }

    public static UserName NewUserName(string value)
    {
        // if (string.IsNullOrWhiteSpace(value))
        // {
        //     //throw new ArgumentNullException(
        //     //    nameof(Value), "User name cannot be empty");

        //     throw new ArgumentException(
        //         nameof(Value), "User name cannot be empty");

        // }

        if (value.Length > MaxLength)
        {
            //throw new ArgumentOutOfRangeException(
            //    nameof(Value), $"User name cannot be longer than {MaxLength} characters");

            throw new ArgumentException(
                nameof(Value), $"User name cannot be longer than {MaxLength} characters");
        }

        return new UserName(value);
    }
    public static UserName FromString(string value) => NewUserName(value);

    public static readonly UserName Empty = new UserName(string.Empty);
    public string Value { get; }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    private const int MaxLength = 50;
}
