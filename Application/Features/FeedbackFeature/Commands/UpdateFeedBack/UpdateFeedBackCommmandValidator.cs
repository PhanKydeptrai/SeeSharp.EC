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
    }
}
