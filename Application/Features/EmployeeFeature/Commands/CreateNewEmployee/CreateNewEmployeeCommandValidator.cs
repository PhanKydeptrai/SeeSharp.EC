using FluentValidation;

namespace Application.Features.EmployeeFeature.Commands.CreateNewEmployee;

internal sealed class CreateNewEmployeeCommandValidator : AbstractValidator<CreateNewEmployeeCommand>
{
    public CreateNewEmployeeCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithErrorCode("UserName.Required")
            .WithMessage("UserName is required.")
            .MaximumLength(50)
            .WithErrorCode("UserName.MaxLength")
            .WithMessage("UserName must not exceed 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithErrorCode("Email.Required")
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithErrorCode("Email.Invalid")
            .WithMessage("Email is not valid.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithErrorCode("PhoneNumber.Required")
            .WithMessage("PhoneNumber is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithErrorCode("PhoneNumber.Invalid")
            .WithMessage("PhoneNumber is not valid.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .WithErrorCode("DateOfBirth.Required")
            .WithMessage("DateOfBirth is required.")
            .Must(date =>
            {
                var today = DateTime.UtcNow;
                var age = today.Year - date!.Value.Year;

                // Kiểm tra xem sinh nhật trong năm nay đã qua chưa
                // Nếu chưa thì trừ đi 1 tuổi
                if (date > DateOnly.FromDateTime(today.AddYears(-age)))
                    age--;

                return age >= 18;
            })
            .WithErrorCode("DateOfBirth.AgeRestriction")
            .WithMessage("You must be at least 18 years old.");
        
        // RuleFor(x => x.Role)
        //     .NotEmpty()
        //     .WithErrorCode("Role.Required")
        //     .WithMessage("Role is required.")
        //     .Must(role => role != "Admin")
        //     .WithErrorCode("Role.Invalid")
        //     .WithMessage("Role cannot be Admin.");
    }
}
