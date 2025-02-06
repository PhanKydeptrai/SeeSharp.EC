using Domain.Primitives;

namespace Domain.Entities.OrderTransactions;

public sealed class IsVoucherUsed : ValueObject
{
    private IsVoucherUsed(bool value) => Value = value;
    public bool Value { get; }
    public static readonly IsVoucherUsed NotUsed = new IsVoucherUsed(false);
    public static readonly IsVoucherUsed Used = new IsVoucherUsed(true);
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
