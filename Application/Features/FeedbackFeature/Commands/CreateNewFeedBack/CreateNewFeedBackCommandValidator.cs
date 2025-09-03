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

        RuleFor(a => a.OrderId)
            .NotEmpty()
            .WithErrorCode("OrderId.Required")
            .WithMessage("OrderId is required");

        RuleFor(a => a.CustomerId)
            .NotEmpty()
            .WithErrorCode("CustomerId.Required")
            .WithMessage("CustomerId is required");

        RuleFor(a => a.IsPrivate)
            .NotEmpty()
            .WithErrorCode("IsPrivate.Required")
            .WithMessage("IsPrivate is required");
    }
}


// public record CreateNewFeedBackCommand(
//     string Substance,
//     int RatingScore,
//     IFormFile? Image,
//     bool IsPrivate,
//     Guid OrderId,
//     Guid CustomerId) : ICommand;