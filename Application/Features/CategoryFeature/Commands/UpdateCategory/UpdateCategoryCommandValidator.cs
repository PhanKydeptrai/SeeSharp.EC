using Application.IServices;
using Domain.Entities.Categories;
using FluentValidation;
using NaughtyStrings;

namespace Application.Features.CategoryFeature.Commands.UpdateCategory;

internal sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator(ICategoryQueryServices categoryQueryServices)
    {
        RuleFor(x => x.categoryId)
            .NotEmpty()
            .WithMessage("CategoryId is required");

        RuleFor(x => x.categoryName)
            .NotEmpty()
            .WithMessage("CategoryName is required")
            .MaximumLength(50)
            .WithMessage("CategoryName must not exceed 50 characters")
            .Must(ValidateInput)
            .WithMessage("Category name contains invalid characters")
            .Must((context, categoryName) =>
            {
                return categoryQueryServices.IsCategoryNameExist(
                    CategoryId.FromGuid(context.categoryId), 
                    CategoryName.FromString(categoryName)).Result == false;
            })
            .WithErrorCode("CategoryName.NotUnique")
            .WithMessage("CategoryName must be unique");
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
