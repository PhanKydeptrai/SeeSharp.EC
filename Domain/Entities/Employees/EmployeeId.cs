using Domain.Primitives;

namespace Domain.Entities.Employees;

public sealed class EmployeeId : ValueObject
{
    private EmployeeId(Ulid value) => Value = value;
    public Ulid Value { get; }
    public static EmployeeId New() => new(Ulid.NewUlid());
    public static EmployeeId FromString(string value) => new(Ulid.Parse(value));
    public static readonly EmployeeId Empty = new(Ulid.Empty);
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

