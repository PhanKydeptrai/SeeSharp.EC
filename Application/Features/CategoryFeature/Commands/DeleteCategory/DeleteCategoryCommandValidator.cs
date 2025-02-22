using FluentValidation;

namespace Application.Features.CategoryFeature.Commands.DeleteCategory;

internal sealed class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(x => x.categoryId)
            .NotEmpty()
            .WithMessage("Category id is required")
            .Must(a => Ulid.TryParse(a, out _))
            .WithMessage("CategoryId is not in the correct format");
    }
}
