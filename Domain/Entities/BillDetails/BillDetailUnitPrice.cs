using Domain.Primitives;

namespace Domain.Entities.Bills;

public sealed class BillDetailUnitPrice : ValueObject
{
    private BillDetailUnitPrice(decimal value) => Value = value;
    public decimal Value { get; }
    
    public static BillDetailUnitPrice NewBillDetailUnitPrice(decimal value) 
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value), "BillDetailUnitPrice cannot be less than or equal to zero.");
        }
        return new BillDetailUnitPrice(value);
    }

    public static BillDetailUnitPrice FromDecimal(decimal value) => new BillDetailUnitPrice(value);

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
