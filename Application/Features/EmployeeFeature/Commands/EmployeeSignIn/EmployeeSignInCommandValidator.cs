using FluentValidation;

namespace Application.Features.EmployeeFeature.Commands.EmployeeSignIn;

internal sealed class EmployeeSignInCommandValidator : AbstractValidator<EmployeeSignInCommand>
{
    public EmployeeSignInCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithErrorCode("Email.Required")
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithErrorCode("Email.Invalid")
            .WithMessage("Invalid email format.");
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(8)
            .WithErrorCode("Password.TooShort")
            .WithMessage("Password must be at least 6 characters long.");
    }
}
