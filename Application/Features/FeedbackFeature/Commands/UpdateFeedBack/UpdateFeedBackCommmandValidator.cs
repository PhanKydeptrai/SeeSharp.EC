using FluentValidation;

namespace Application.Features.FeedbackFeature.Commands.UpdateFeedBack;

internal sealed class UpdateFeedBackCommmandValidator : AbstractValidator<UpdateFeedBackCommmand>
{
    public UpdateFeedBackCommmandValidator()
    {
        RuleFor(a => a.Substance)
            .NotEmpty()
            .WithErrorCode("Substance.Required")
            .WithMessage("Substance is required.")
            .MaximumLength(500)
            .WithMessage("Substance must not exceed 500 characters.");

        RuleFor(a => a.RatingScore)
            .NotEmpty()
            .WithErrorCode("RatingScore.Required")
            .WithMessage("RatingScore is required")
            .InclusiveBetween(1, 5)
            .WithErrorCode("RatingScore.Range")
            .WithMessage("RatingScore must be between 1 and 5");
    }
}
