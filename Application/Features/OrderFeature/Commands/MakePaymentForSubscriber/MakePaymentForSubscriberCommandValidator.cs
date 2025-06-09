using Domain.IRepositories.ShippingInformations;
using FluentValidation;

namespace Application.Features.OrderFeature.Commands.MakePaymentForSubscriber;

internal sealed class MakePaymentForSubscriberCommandValidator : AbstractValidator<MakePaymentForSubscriberCommand>
{
    public MakePaymentForSubscriberCommandValidator(IShippingInformationRepository shippingInformationRepository)
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithErrorCode("CustomerId.Required")
            .WithMessage("Customer ID is required");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithErrorCode("FullName.Required")
            .WithMessage("Full name is required")
            .MaximumLength(50)
            .WithErrorCode("FullName.TooLong")
            .WithMessage("Full name must not exceed 50 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithErrorCode("PhoneNumber.Required")
            .WithMessage("Phone number is required")
            .MaximumLength(10)
            .WithErrorCode("PhoneNumber.TooLong")
            .WithMessage("Phone number must not exceed 10 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithErrorCode("Email.Required")
            .WithMessage("Email is required")
            .EmailAddress()
            .WithErrorCode("Email.Invalid")
            .WithMessage("Email format is invalid")
            .MaximumLength(50)
            .WithErrorCode("Email.TooLong")
            .WithMessage("Email must not exceed 100 characters");

        RuleFor(x => x.Province)
            .NotEmpty()
            .WithErrorCode("Province.Required")
            .WithMessage("Province is required")
            .MaximumLength(50)
            .WithErrorCode("Province.TooLong")
            .WithMessage("Province must not exceed 50 characters");

        RuleFor(x => x.District)
            .NotEmpty()
            .WithErrorCode("District.Required")
            .WithMessage("District is required")
            .MaximumLength(50)
            .WithErrorCode("District.TooLong")
            .WithMessage("District must not exceed 50 characters");

        RuleFor(x => x.Ward)
            .NotEmpty()
            .WithErrorCode("Ward.Required")
            .WithMessage("Ward is required")
            .MaximumLength(50)
            .WithErrorCode("Ward.TooLong")
            .WithMessage("Ward must not exceed 50 characters");

        RuleFor(x => x.SpecificAddress)
            .NotEmpty()
            .WithErrorCode("SpecificAddress.Required")
            .WithMessage("Specific address is required")
            .MaximumLength(255)
            .WithErrorCode("SpecificAddress.TooLong")
            .WithMessage("Specific address must not exceed 255 characters");
    }
}