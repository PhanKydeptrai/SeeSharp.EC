using Domain.Primitives;

namespace Domain.Entities.Categories;
public sealed class CategoryId : ValueObject
{
    private CategoryId(Ulid value) => Value = value;
    public Ulid Value { get; }
    public static CategoryId New() => new(Ulid.NewUlid());
    public static CategoryId FromString(string value) => new(Ulid.Parse(value));
    public static readonly CategoryId Empty = new(Ulid.Empty);
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
