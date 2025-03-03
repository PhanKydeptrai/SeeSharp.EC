using FluentValidation;

namespace Application.Features.CustomerFeature.Commands.CustomerSignIn;

internal sealed class CustomerSignInCommandValidator : AbstractValidator<CustomerSignInCommand>
{
    public CustomerSignInCommandValidator()
    {
        RuleFor(a => a.Email)
            .NotEmpty()
            .WithErrorCode("Email.IsRequired")
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithErrorCode("Email.IsInvalid")
            .WithMessage("Email is invalid.");

        RuleFor(a => a.Password)
            .NotEmpty()
            .WithErrorCode("Password.IsRequired")
            .WithMessage("Password is required.")
            .MinimumLength(8)
            .WithErrorCode("Password.MinimumLength")
            .WithMessage("Password must be at least 8 characters long.");
    }
}
