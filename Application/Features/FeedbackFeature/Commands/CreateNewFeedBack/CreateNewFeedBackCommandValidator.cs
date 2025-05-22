using FluentValidation;

namespace Application.Features.FeedbackFeature.Commands.CreateNewFeedBack;

internal class CreateNewFeedBackCommandValidator : AbstractValidator<CreateNewFeedBackCommand>
{
    public CreateNewFeedBackCommandValidator()
    {
        RuleFor(a => a.Substance)
            .NotEmpty()
            .WithErrorCode("Substance.Required")
            .WithMessage("Substance is required");
        
        RuleFor(a => a.RatingScore)
            .NotEmpty()
            .WithErrorCode("RatingScore.Required")
            .WithMessage("RatingScore is required")
            .InclusiveBetween(1, 5)
            .WithErrorCode("RatingScore.Range")
            .WithMessage("RatingScore must be between 1 and 5");
    }
}
