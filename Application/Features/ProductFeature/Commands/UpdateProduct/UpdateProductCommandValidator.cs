using Application.IServices;
using Domain.Entities.Categories;
using Domain.Entities.Products;
using FluentValidation;
using NaughtyStrings;

namespace Application.Features.ProductFeature.Commands.UpdateProduct;
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator(
        IProductQueryServices productQueryServices, 
        ICategoryQueryServices categoryQueryServices)
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithErrorCode("ProductId.NotEmpty")
            .WithMessage("ProductId is required");
        
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithErrorCode("CategoryId.NotEmpty")
            .WithMessage("CategoryId is required");

        RuleFor(x => x.ProductName)
            .NotEmpty()
            .WithErrorCode("ProductName.NotEmpty")
            .WithMessage("ProductName is required")
            .MaximumLength(50)
            .WithErrorCode("ProductName.MaxLength")
            .WithMessage("ProductName must not exceed 50 characters")
            .Must((context, productName) =>
            {
                return productQueryServices.IsProductNameExist(
                    ProductId.FromGuid(context.ProductId), 
                    ProductName.FromString(productName)).Result is false;
            })
            .WithErrorCode("ProductName.NotUnique")
            .WithMessage("Prodyuct name must be unique");

        RuleFor(x => x.ColorCode)
            .NotEmpty()
            .WithErrorCode("ColorCode.IsRequied")
            .WithMessage("ColorCode is required");
            // .Must(x => x.StartsWith("#") && x.Length == 7)
            // .WithErrorCode("ColorCode.Invalid")
            // .WithMessage("Color code must be in hex format (#RRGGBB)");

        RuleFor(x => x.Description)
            .MaximumLength(250)
            .WithErrorCode("ProductName.MaxLength")
            .WithMessage("ProductName must not exceed 50 characters");

        RuleFor(x => x.ProductPrice)
            .NotEmpty()
            .WithErrorCode("ProductPrice.NotEmpty")
            .WithMessage("ProductPrice is required")
            .Must(productPrice => productPrice > 0)
            .WithErrorCode("ProductPrice.Invalid")
            .WithMessage("ProductPrice must be greater than 0");

        RuleFor(x => x.CategoryId)
            .Must(x => categoryQueryServices.IsCategoryStatusNotDeleted(CategoryId.FromGuid(x!.Value)).Result is true)
            .When(x => x.CategoryId is not null)
            .WithErrorCode("CategoryId.Invalid")
            .WithMessage("CategoryId is invalid or deleted");

    }
    // private bool ValidateInput(string input)
    // {
    //     foreach (var naughtyString in TheNaughtyStrings.All)
    //     {
    //         if (input.Contains(naughtyString))
    //         {
    //             return false;
    //         }
    //     }
    //     return true;
    // }

}
