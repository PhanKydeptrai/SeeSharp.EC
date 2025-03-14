using Application.IServices;
using Domain.Entities.Products;
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

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithErrorCode("ProductId.Required")
            .WithMessage("ProductId is required")
            .Must(x => productQueryServices.IsProductExist(ProductId.FromGuid(x)).Result);
    }
}
