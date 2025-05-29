using FluentValidation;

namespace Application.Features.FeedbackFeature.Commands.CreateNewFeedBack;

internal class CreateNewFeedBackCommandValidator : AbstractValidator<CreateNewFeedBackCommand>
{
    public CreateNewFeedBackCommandValidator()
    {
        RuleFor(a => a.Substance)
            .NotEmpty()
            .WithErrorCode("Substance.Required")
            .WithMessage("Substance is required")
            .MaximumLength(500)
            .WithErrorCode("Substance.TooLong")
            .WithMessage("Substance is too long");
        
        RuleFor(a => a.RatingScore)
            .NotEmpty()
            .WithErrorCode("RatingScore.Required")
            .WithMessage("RatingScore is required")
            .InclusiveBetween(1, 5)
            .WithErrorCode("RatingScore.Range")
            .WithMessage("RatingScore must be between 1 and 5");
    }
}
