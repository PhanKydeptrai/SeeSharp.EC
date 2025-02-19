using Domain.Entities.Categories;
using Domain.IRepositories.CategoryRepositories;
using FluentValidation;

namespace Application.Features.CategoryFeature.Commands.UpdateCategory;

internal sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator(ICategoryRepository categoryRepository)
    {
        RuleFor(x => x.categoryId)
            .NotEmpty()
            .WithMessage("CategoryId is required")
            .Must(a => Ulid.TryParse(a, out _))
            .WithMessage("CategoryId is not in the correct format");

        RuleFor(x => x.categoryName) //!FIXME: Move to handler
            .NotEmpty()
            .WithMessage("CategoryName is required")
            .MaximumLength(50)
            .WithMessage("CategoryName must not exceed 50 characters")
            .Must((id, name) => categoryRepository.IsCategoryNameExistWhenUpdate(
                CategoryId.FromString(id.categoryId), 
                CategoryName.FromString(name)).Result == false)
            .WithMessage("CategoryName already exists");
        
    }
}
