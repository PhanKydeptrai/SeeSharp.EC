using System.Text.RegularExpressions;
using Domain.Primitives;

namespace Domain.Entities.Users;

public sealed class PhoneNumber : ValueObject
{
    private PhoneNumber(string value)
    { 
        Value = value;
    }

    public static PhoneNumber NewPhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            
            throw new ArgumentException(
                nameof(value), "Phone number cannot be empty");
        }

        if (!Regex.IsMatch(value, PhoneNumberRegex))
        {
            throw new ArgumentException(
                nameof(value), "Phone number is invalid");
        }
        return new PhoneNumber(value);
    }

    public static PhoneNumber FromString(string value) => new PhoneNumber(value);
    public readonly static PhoneNumber Empty = new PhoneNumber(string.Empty);

    public string Value { get; }
    private const string PhoneNumberRegex = @"^([0-9]{10})$";
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
