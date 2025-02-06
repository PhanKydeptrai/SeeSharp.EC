using Domain.Primitives;

namespace Domain.Entities.CustomerVouchers;

public sealed class CustomerVoucherQuantity : ValueObject
{
    private CustomerVoucherQuantity(int value) => Value = value;
    public int Value { get; }
    public static CustomerVoucherQuantity NewCustomerVoucherQuantity(int quantity)
    {
        if (quantity < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantity), "Quantity cannot be negative");
        }
        return new CustomerVoucherQuantity(quantity);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
