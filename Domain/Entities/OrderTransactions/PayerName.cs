using Domain.Primitives;

namespace Domain.Entities.OrderTransactions;

public sealed class PayerName : ValueObject
{
    private PayerName(string value)
    {
        Value = value;
    }
    public string Value { get; }
    public static PayerName NewPayerName(string payerName)
    {
        if (string.IsNullOrEmpty(payerName))
        {
            throw new ArgumentNullException(
                nameof(payerName), "Payer name is empty");
        }
        if (payerName.Length > MaxLengh)
        {

            throw new ArgumentOutOfRangeException(
                nameof(payerName), "Payer name is too long");
        }

        return new PayerName(payerName);
    }
    public static PayerName Empty => new PayerName(string.Empty);
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    //* Constants
    public const int MaxLengh = 50;
}