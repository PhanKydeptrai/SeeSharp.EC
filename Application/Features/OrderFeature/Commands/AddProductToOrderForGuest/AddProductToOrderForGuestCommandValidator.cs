using System;
using FluentValidation;

namespace Application.Features.OrderFeature.Commands.AddProductToOrderForGuest;

internal sealed class AddProductToOrderForGuestCommandValidator : AbstractValidator<AddProductToOrderForGuestCommand>
{
    public AddProductToOrderForGuestCommandValidator()
    {
        RuleFor(x => x.ProductVariantId)
            .NotEmpty()
            .WithErrorCode("ProductVariantId.Empty")
            .WithMessage("ProductVariantId is required");

        RuleFor(x => x.Quantity)
            .NotEmpty()
            .WithErrorCode("Quantity.Empty")
            .WithMessage("Quantity is required")
            .GreaterThan(0)
            .WithErrorCode("Quantity.Invalid")
            .WithMessage("Quantity must be greater than 0");
    }
}
