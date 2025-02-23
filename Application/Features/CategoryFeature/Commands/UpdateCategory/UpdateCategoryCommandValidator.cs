using Domain.IRepositories.CategoryRepositories;
using FluentValidation;
using NaughtyStrings;

namespace Application.Features.CategoryFeature.Commands.UpdateCategory;

internal sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator(ICategoryRepository categoryRepository)
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
            .WithMessage("Category name contains invalid characters");
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
