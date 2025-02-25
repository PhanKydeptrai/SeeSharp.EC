using Application.IServices;
using Domain.Entities.Products;
using FluentValidation;
using NaughtyStrings;

namespace Application.Features.ProductFeature.Commands.UpdateProduct;
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator(IProductQueryServices productQueryServices)
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
                    ProductName.FromString(productName)).Result == false; //TEST
            })
            .WithErrorCode("ProductName.NotUnique")
            .WithMessage("Prodyuct name must be unique")
            .Must(ValidateInput)
            .WithErrorCode("ProductName.Invalid")
            .WithMessage("Product name contains invalid characters");

        RuleFor(x => x.Description)
            .MaximumLength(250)
            .WithErrorCode("ProductName.MaxLength")
            .WithMessage("ProductName must not exceed 50 characters");

        RuleFor(x => x.ProductPrice)
            .NotEmpty()
            .WithErrorCode("ProductPrice.NotEmpty")
            .WithMessage("ProductPrice is required")
            .Must(productPrice => productPrice > 0);

    }
    private bool ValidateInput(string input)
    {
        foreach (var naughtyString in TheNaughtyStrings.All)
        {
            if (input.Contains(naughtyString))
            {
                return false;
            }
        }
        return true;
    }

}
