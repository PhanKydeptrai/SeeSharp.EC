using Domain.Primitives;

namespace Domain.Entities.Feedbacks;

public sealed class RatingScore : ValueObject
{
    private RatingScore(float value) => Value = value;
    public float Value { get; }
    public static RatingScore NewRatingScore(float value)
    {
        if (value < 0 || value > 5)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value), "Rating score must be between 0 and 5");
        }
        return new RatingScore(value);
    }
    public static RatingScore FromFloat(float value) => new RatingScore(value);
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}