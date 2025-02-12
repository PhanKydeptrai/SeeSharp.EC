using Domain.Primitives;

namespace Domain.Entities.Bills;


public sealed class BillId : BaseId<BillId>;


#region Old Implementation
//public sealed class BillId : ValueObject
//{
//    private BillId(Ulid value) => Value = value;
//    public Ulid Value { get; }
//    public static BillId New() => new BillId(Ulid.NewUlid());
//    public static BillId FromString(string value) => new BillId(Ulid.Parse(value));
//    public static readonly BillId Empty = new BillId(Ulid.Empty);
//    public override IEnumerable<object> GetAtomicValues()
//    {
//        yield return Value;
//    }
//}

#endregion