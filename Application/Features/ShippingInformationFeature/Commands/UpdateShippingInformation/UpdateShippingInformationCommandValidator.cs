using FluentValidation;

namespace Application.Features.ShippingInformationFeature.Commands.UpdateShippingInformation;

public sealed class UpdateShippingInformationCommandValidator : AbstractValidator<UpdateShippingInformationCommand>
{
    public UpdateShippingInformationCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithErrorCode("CustomerId.Required")
            .WithMessage("Customer ID is required.");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithErrorCode("FullName.Required")
            .WithMessage("Full name is required.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches(@"^\+?[1-9]\d{1,14}$") // Basic phone number validation
            .WithMessage("Phone number is required.");

        RuleFor(x => x.Province)
            .NotEmpty()
            .WithErrorCode("Province.Required")
            .WithMessage("Province is required.");

        RuleFor(x => x.District)
            .NotEmpty()
            .WithErrorCode("District.Required")
            .WithMessage("District is required.");

        RuleFor(x => x.Ward)
            .NotEmpty()
            .WithErrorCode("Ward.Required")
            .WithMessage("Ward is required.");

        RuleFor(x => x.SpecificAddress)
            .NotEmpty()
            .WithErrorCode("SpecificAddress.Required")
            .WithMessage("Specific address is required.");
    }
}