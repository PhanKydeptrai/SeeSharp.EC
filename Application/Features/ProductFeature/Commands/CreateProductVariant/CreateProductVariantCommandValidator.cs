using Application.IServices;
using Domain.Entities.Products;
using Domain.Entities.ProductVariants;
using FluentValidation;

namespace Application.Features.ProductFeature.Commands.CreateProductVariant;

internal sealed class CreateProductVariantCommandValidator : AbstractValidator<CreateProductVariantCommand>
{
    public CreateProductVariantCommandValidator(IProductQueryServices productQueryServices)
    {
        RuleFor(x => x.VariantName)
            .NotEmpty()
            .WithErrorCode("ProductVariantName.Required")
            .WithMessage("Product variant name is required.")
            .MaximumLength(50)
            .WithErrorCode("ProductVariantName.MaxLength")
            .WithMessage("Product variant name must not exceed 100 characters.")
            .Must((entity, variantName) =>
            {
                return !productQueryServices.IsProductVariantNameExist(
                    ProductId.FromGuid(entity.ProductId), 
                    null, VariantName.FromString(variantName)).Result;
            })
            .WithErrorCode("ProductVariantName.NotUnique")
            .WithMessage("Product variant name must be unique.");
        
        RuleFor(x => x.ProductVariantPrice)
            .NotEmpty()
            .WithErrorCode("VariantPrice.IsRequied")
            .WithMessage("VariantPrice is required")
            .GreaterThan(0)
            .WithErrorCode("VariantPrice.MustGreaterThanZero")
            .WithMessage("VariantPrice is lower than 0");
        
        RuleFor(x => x.ColorCode)
            .NotEmpty()
            .WithErrorCode("ColorCode.IsRequied")
            .WithMessage("ColorCode is required")
            .Must(x => x.StartsWith("#") && x.Length == 7)
            .WithErrorCode("ColorCode.Invalid")
            .WithMessage("Color code must be in hex format (#RRGGBB)");

        RuleFor(x => x.ProductVariantDescription)
            .NotEmpty()
            .WithErrorCode("ProductVariantDescription.Required")
            .WithMessage("Product variant description is required.")
            .MaximumLength(250)
            .WithErrorCode("ProductVariantDescription.MaxLength")
            .WithMessage("Product variant description must not exceed 250 characters.");
    }
}
