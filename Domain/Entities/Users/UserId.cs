using Domain.Primitives;

namespace Domain.Entities.Users;
public sealed class UserId : ValueObject
{
    private UserId(Ulid value) => Value = value;
    public Ulid Value { get; }
    public static UserId New() => new(Ulid.NewUlid());
    public static UserId FromString(string value) => new(Ulid.Parse(value));
    public static readonly UserId Empty = new(Ulid.Empty);
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}   
