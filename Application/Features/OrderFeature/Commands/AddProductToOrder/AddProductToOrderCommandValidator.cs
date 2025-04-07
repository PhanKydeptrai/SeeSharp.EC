using Application.Features.OrderFeature.Commands.AddProductToOrder;
using FluentValidation;

namespace Application.Features.OrderFeature.Commands;

internal sealed class AddProductToOrderCommandValidator : AbstractValidator<AddProductToOrderCommand>
{
    public AddProductToOrderCommandValidator()
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
