using Application.IServices;
using Domain.Entities.Products;
using Domain.Entities.ProductVariants;
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
            .Must(x => !productQueryServices.IsProductNameExist(null, ProductName.FromString(x)).Result)
            .WithErrorCode("ProductName.NotUnique")
            .WithMessage("Product name must be unique");

        RuleFor(x => x.ProductBaseVariantName)
            .NotEmpty()
            .WithErrorCode("ProductBaseVariantName.IsRequied")
            .WithMessage("Product name is required")
            .MaximumLength(50)
            .WithErrorCode("ProductBaseVariantName.TooLong")
            .WithMessage("Product name is too long");
            
        RuleFor(x => x.ColorCode)
            .NotEmpty()
            .WithErrorCode("ColorCode.IsRequied")
            .WithMessage("ColorCode is required")
            .Must(x => x.StartsWith("#") && x.Length == 7)
            .WithErrorCode("ColorCode.Invalid")
            .WithMessage("Color code must be in hex format (#RRGGBB)");
        
        RuleFor(x => x.VariantPrice)
            .NotEmpty()
            .WithErrorCode("VariantPrice.IsRequied")
            .WithMessage("VariantPrice is required")
            .GreaterThan(0)
            .WithErrorCode("VariantPrice.MustGreaterThanZero")
            .WithMessage("VariantPrice is lower than 0");

    }
}
