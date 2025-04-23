using Domain.Entities.Feedbacks;
using Xunit;

namespace SeeSharp.EC.Application.UnitTest.FeedbackTest;

public class FeedbackValueObjectsTests
{
    [Theory]
    [InlineData(0.0f, true)] // Minimum valid score
    [InlineData(5.0f, true)] // Maximum valid score
    [InlineData(2.5f, true)] // Valid score
    [InlineData(-0.1f, false)] // Below minimum
    [InlineData(5.1f, false)] // Above maximum
    public void RatingScore_Validation_Should_Work_Correctly(float score, bool isValid)
    {
        if (isValid)
        {
            var ratingScore = RatingScore.NewRatingScore(score);
            Assert.NotNull(ratingScore);
            Assert.Equal(score, ratingScore.Value);
        }
        else
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => RatingScore.NewRatingScore(score));
        }
    }

    [Theory]
    [InlineData("Valid feedback content", true)]
    [InlineData("", false)] // Empty
    [InlineData("   ", false)] // Whitespace
    [InlineData("a", true)] // Minimum length
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", true)] // Maximum length (50 chars)
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", false)] // Too long (51 chars)
    public void Substance_Validation_Should_Work_Correctly(string content, bool isValid)
    {
        if (isValid)
        {
            var substance = Substance.NewSubstance(content);
            Assert.NotNull(substance);
            Assert.Equal(content, substance.Value);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => Substance.NewSubstance(content));
        }
    }

    [Fact]
    public void Substance_Empty_Should_Work_Correctly()
    {
        var emptySubstance = Substance.Empty;
        Assert.NotNull(emptySubstance);
        Assert.Equal(string.Empty, emptySubstance.Value);
    }
} 