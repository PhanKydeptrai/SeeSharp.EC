using Domain.Entities.Categories;
using Domain.IRepositories.CategoryRepositories;
using FluentValidation;

namespace Application.Features.CategoryFeature.Commands.CreateCategory;

internal sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator(ICategoryRepository categoryRepository)
    {
        RuleFor(x => x.categoryName)
            .NotEmpty()
            .WithMessage("Category name is required")
            .MaximumLength(50)
            .WithMessage("Category name must not exceed 50 characters")
            .Must(a => categoryRepository.IsCategoryNameExist(CategoryName.FromString(a)).Result == false)
            .WithMessage("Category name already exists");
    }
}
