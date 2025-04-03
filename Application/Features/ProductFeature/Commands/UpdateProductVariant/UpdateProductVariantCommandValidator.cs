using Application.IServices;
using Domain.Entities.Products;
using Domain.Entities.ProductVariants;
using FluentValidation;

namespace Application.Features.ProductFeature.Commands.UpdateProductVariant;

internal sealed class UpdateProductVariantCommandValidator : AbstractValidator<UpdateProductVariantCommand>
{
    public UpdateProductVariantCommandValidator(IProductQueryServices productQueryServices)
    {
        RuleFor(x => x.VariantName)
            .NotEmpty()
            .WithErrorCode("VariantName.Required")
            .WithMessage("Variant name is required.")
            .MaximumLength(100)
            .WithMessage("Variant name must not exceed 100 characters.")
            .Must((entity, variantName) =>
            {
                return !productQueryServices.IsProductVariantNameExist(
                    ProductId.FromGuid(entity.ProductId), 
                    ProductVariantId.FromGuid(entity.ProductVariantId), 
                    VariantName.FromString(variantName)).Result;
            });
        
        RuleFor(x => x.ProductVariantPrice)
            .NotEmpty()
            .WithErrorCode("ProductVariantPrice.Required")
            .WithMessage("Product variant price is required.")
            .GreaterThan(0)
            .WithMessage("Product variant price must be greater than 0.");

        RuleFor(x => x.ColorCode)
            .NotEmpty()
            .WithErrorCode("ColorCode.IsRequied")
            .WithMessage("ColorCode is required")
            .Must(x => x.StartsWith("#") && x.Length == 7)
            .WithErrorCode("ColorCode.Invalid")
            .WithMessage("Color code must be in hex format (#RRGGBB)");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithErrorCode("Description.Required")
            .WithMessage("Description is required.")
            .MaximumLength(250)
            .WithErrorCode("Description.TooLong")
            .WithMessage("Description must not exceed 500 characters.");
        
    }
}
