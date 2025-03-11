using FluentValidation;

namespace Application.Features.OrderFeature.Commands;

internal sealed class AddProductToOrderCommandValidator : AbstractValidator<AddProductToOrderCommand>
{
    public AddProductToOrderCommandValidator()
    {   
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithErrorCode("ProductId.Empty")
            .WithMessage("ProductId is required");

        RuleFor(x => x.Quantity)        
            .NotEmpty()
            .WithErrorCode("Quantity.Empty")
            .WithMessage("Quantity is required")
            .GreaterThan(0)
            .WithErrorCode("Quantity.Invalid")
            .WithMessage("Quantity must be greater than 0");

        
    }
}
