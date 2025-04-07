using Application.IServices;
using Domain.Entities.ProductVariants;
using FluentValidation;

namespace Application.Features.WishItemFeature.Commands;

internal sealed class AddWishListCommandValidator : AbstractValidator<AddWishListCommand>
{
    public AddWishListCommandValidator(IProductQueryServices productQueryServices)
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithErrorCode("CustomerId.Required")
            .WithMessage("CustomerId is required");

        RuleFor(x => x.ProductVariantId)
            .NotEmpty()
            .WithErrorCode("ProductId.Required")
            .WithMessage("ProductId is required")
            .Must(x => productQueryServices.IsProductVariantExist(ProductVariantId.FromGuid(x)).Result);
    }
}
