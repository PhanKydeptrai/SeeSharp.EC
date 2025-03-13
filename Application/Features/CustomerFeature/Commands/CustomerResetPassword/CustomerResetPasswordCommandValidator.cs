using FluentValidation;

namespace Application.Features.CustomerFeature.Commands.CustomerResetPassword;

internal sealed class CustomerResetPasswordCommandValidator : AbstractValidator<CustomerResetPasswordCommand>
{
    public CustomerResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithErrorCode("Email.Required")
            .WithMessage("Email is required")
            .EmailAddress()
            .WithErrorCode("Email.Invalid")
            .WithMessage("Email is not valid")
            .MaximumLength(50)
            .WithErrorCode("Email.MaximumLength")
            .WithMessage("Email must not exceed 50 characters");
    }
}
