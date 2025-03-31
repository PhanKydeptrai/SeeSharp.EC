using Application.IServices;
using FluentValidation;

namespace Application.Features.CustomerFeature.Commands.CustomerSignUp;

internal sealed class CustomerSignUpCommandValidator : AbstractValidator<CustomerSignUpCommand>
{
    public CustomerSignUpCommandValidator(ICustomerQueryServices customerQueryServices)
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithErrorCode("Email.Required")
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithErrorCode("Email.Invalid")
            .WithMessage("Email is invalid.")
            // .Must(a => customerQueryServices.IsCustomerEmailExist(null, Email.NewEmail(a)).Result is false)
            // .WithErrorCode("Email.Exist")
            // .WithMessage("Email already exists.")
            .MaximumLength(50)
            .WithErrorCode("Email.MaximumLength")
            .WithMessage("Email must not exceed 50 characters.");

        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithErrorCode("UserName.Required")
            .WithMessage("UserName is required.")
            .MaximumLength(50)
            .WithErrorCode("UserName.MaximumLength")
            .WithMessage("UserName must not exceed 50 characters.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithErrorCode("Password.Required")
            .WithMessage("Password is required.")
            .MinimumLength(8)
            .WithErrorCode("Password.MinimumLength")
            .WithMessage("Password must be at least 8 characters long.");
    }
}
