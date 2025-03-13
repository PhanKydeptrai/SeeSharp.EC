using FluentValidation;

namespace Application.Features.CustomerFeature.Commands.CustomerChangePassword;

internal sealed class CustomerChangePasswordCommandValidator : AbstractValidator<CustomerChangePasswordCommand>
{
    public CustomerChangePasswordCommandValidator()
    {
        RuleFor(x => x.userId)
            .NotEmpty()
            .WithErrorCode("UserId.Empty")
            .WithMessage("UserId is required");

        RuleFor(x => x.currentPassword)
            .NotEmpty()
            .WithErrorCode("CurrentPassword.Empty")
            .WithMessage("Current password is required");

        RuleFor(x => x.newPassword)
            .NotEmpty()
            .WithErrorCode("NewPassword.Empty")
            .WithMessage("New password is required")
            .MinimumLength(8)
            .WithErrorCode("NewPassword.MinLength")
            .WithMessage("New password must be at least 8 characters long")
            .MaximumLength(50)
            .WithErrorCode("NewPassword.MaxLength")
            .WithMessage("New password must be at most 50 characters long");

        RuleFor(x => x.repeatNewPassword)
            .NotEmpty()
            .WithErrorCode("RepeatNewPassword.Empty")
            .WithMessage("Repeat new password is required")
            .Equal(x => x.newPassword)
            .WithErrorCode("RepeatNewPassword.NotEqual")
            .WithMessage("Repeat new password must be equal to new password");
    }
}
