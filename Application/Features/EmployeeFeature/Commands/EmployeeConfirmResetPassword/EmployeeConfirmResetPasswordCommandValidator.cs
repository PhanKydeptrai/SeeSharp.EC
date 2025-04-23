using FluentValidation;

namespace Application.Features.EmployeeFeature.Commands.EmployeeConfirmResetPassword;

internal sealed class EmployeeConfirmResetPasswordCommandValidator : AbstractValidator<EmployeeConfirmResetPasswordCommand>
{
    public EmployeeConfirmResetPasswordCommandValidator()
    {
        RuleFor(x => x.token)
            .NotEmpty()
            .WithErrorCode("Token.Required")
            .WithMessage("Token is required.");
    }
} 