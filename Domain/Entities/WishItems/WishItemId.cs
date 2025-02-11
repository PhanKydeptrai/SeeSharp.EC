using Domain.Primitives;

namespace Domain.Entities.WishItems;
public sealed class WishItemId : ValueObject
{
    private WishItemId(Ulid value) => Value = value;
    public Ulid Value { get; }
    public static WishItemId New() => new(Ulid.NewUlid());
    public static WishItemId FromString(string value) => new(Ulid.Parse(value));
    public static readonly WishItemId Empty = new(Ulid.Empty);
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}