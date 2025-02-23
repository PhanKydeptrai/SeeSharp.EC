namespace Domain.Primitives;

public abstract class BaseId<T> : ValueObject where T : BaseId<T> , new()
{
    public Guid Value { get; private set; }

    //trả giá trị trực tiếp không cần gọi Value
    public static implicit operator Guid(BaseId<T> baseId)
    {
        return baseId.Value;
    }
    public static T New()
    {
        var id = new T();
        id.Value = Ulid.NewUlid().ToGuid();
        return id;
    }

    public static T FromString(string value)
    {
        var id = new T();
        id.Value = string.IsNullOrWhiteSpace(value) ? Guid.Empty : Guid.Parse(value);
        return id;
    }

    public static T FromGuid(Guid value)
    {
        var id = new T();
        id.Value = value;
        return id;
    }

    public static T FromUlid(Ulid value)
    {
        var id = new T();
        id.Value = value.ToGuid();
        return id;
    }

    public static readonly T Empty = CreateEmpty();

    private static T CreateEmpty()
    {
        var id = new T();
        id.Value = Guid.Empty;
        return id;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
