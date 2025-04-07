using FluentValidation;

namespace Application.Features.EmployeeFeature.Commands.EmployeeConfirmChangePassword;

internal sealed class EmployeeConfirmChangePasswordCommandValidator : AbstractValidator<EmployeeConfirmChangePasswordCommand>
{
    public EmployeeConfirmChangePasswordCommandValidator()
    {
        RuleFor(x => x.verificationTokenId)
            .NotEmpty()
            .WithErrorCode("VerificationToken.Required")
            .WithMessage("Verification token is required.");
    }
} 