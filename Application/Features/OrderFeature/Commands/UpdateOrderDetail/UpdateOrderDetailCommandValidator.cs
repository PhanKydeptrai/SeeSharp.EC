using FluentValidation;

namespace Application.Features.OrderFeature.Commands.UpdateOrderDetail;

internal sealed class UpdateOrderDetailCommandValidator : AbstractValidator<UpdateOrderDetailCommand>
{
    public UpdateOrderDetailCommandValidator()
    {
        RuleFor(a => a.Quantity)
            .NotEmpty()
            .WithErrorCode("Quantity.Required")
            .WithMessage("Quantity is required")
            .GreaterThan(0)
            .WithErrorCode("Quantity.GreaterThanZero")
            .WithMessage("Quantity must be greater than 0");
    }
}
