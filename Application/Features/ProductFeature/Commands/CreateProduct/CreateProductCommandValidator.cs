using Application.IServices;
using Domain.Entities.Products;
using FluentValidation;

namespace Application.Features.ProductFeature.Commands.CreateProduct;

internal sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator(IProductQueryServices productQueryServices)
    {
        RuleFor(x => x.ProductName)
            .NotEmpty()
            .WithErrorCode("ProductName.IsRequied")
            .WithMessage("Product name is required")
            .MaximumLength(50)
            .WithErrorCode("ProductName.TooLong")
            .WithMessage("Product name is too long")
            .Must(x => !productQueryServices.IsProductNameExist(null,ProductName.FromString(x)).Result)
            .WithErrorCode("ProductName.NotUnique")
            .WithMessage("Product name must be unique");

        RuleFor(x => x.Price)
            .NotEmpty()
            .WithErrorCode("Price.IsRequied")
            .WithMessage("Price is required")
            .GreaterThan(0)
            .WithErrorCode("Price.MustGreaterThanZero")
            .WithMessage("Price is lower than 0");

    }
}
