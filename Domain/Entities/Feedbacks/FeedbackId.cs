using Domain.Primitives;

namespace Domain.Entities.Feedbacks;
public class FeedbackId : ValueObject
{
    private FeedbackId(Ulid value) => Value = value;
    public Ulid Value { get; }
    public static FeedbackId New() => new(Ulid.NewUlid());
    public static FeedbackId FromString(string value) => new(Ulid.Parse(value));
    public static readonly FeedbackId Empty = new(Ulid.Empty);
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}