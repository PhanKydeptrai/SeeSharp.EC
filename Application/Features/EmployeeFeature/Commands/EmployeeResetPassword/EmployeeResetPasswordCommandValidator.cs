using FluentValidation;

namespace Application.Features.EmployeeFeature.Commands.EmployeeResetPassword;

internal sealed class EmployeeResetPasswordCommandValidator : AbstractValidator<EmployeeResetPasswordCommand>
{
    public EmployeeResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithErrorCode("Email.Required")
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithErrorCode("Email.Invalid")
            .WithMessage("Email is not valid.");
    }
} 