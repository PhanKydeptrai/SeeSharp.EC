using Application.IServices;
using Domain.Entities.Categories;
using FluentValidation;
using NaughtyStrings;

namespace Application.Features.CategoryFeature.Commands.CreateCategory;

internal sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator(ICategoryQueryServices categoryQueryServices)
    {
        RuleFor(x => x.categoryName)
            .NotEmpty()
            .WithMessage("Category name is required")
            .MaximumLength(50)
            .WithMessage("Category name must not exceed 50 characters")
            .Must(a => categoryQueryServices.IsCategoryNameExist(null,CategoryName.FromString(a)).Result == false)
            .WithMessage("Category name already exists");
            // .Must(ValidateInput)
            // .WithMessage("Category name contains invalid characters");
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
