using Domain.Entities.Users;
using FluentValidation;

namespace Application.Features.EmployeeFeature.Commands.UpdateEmployeeStatus;

public class UpdateEmployeeStatusCommandValidator : AbstractValidator<UpdateEmployeeStatusCommand>
{
    public UpdateEmployeeStatusCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithErrorCode("EmployeeId.Required")
            .WithMessage("Employee ID is required");

        RuleFor(x => x.NewStatus)
            .NotEmpty()
            .WithErrorCode("Status.Required")
            .WithMessage("Status is required")
            .Must(BeValidStatus)
            .WithErrorCode("Status.Invalid")
            .WithMessage("Invalid status. Valid values are: Active, InActive, Deleted, Blocked");
    }

    private bool BeValidStatus(string status)
    {
        return Enum.TryParse<UserStatus>(status, true, out _);
    }
}