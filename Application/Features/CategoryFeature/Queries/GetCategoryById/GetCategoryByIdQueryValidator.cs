using FluentValidation;

namespace Application.Features.CategoryFeature.Queries.GetCategoryById;

internal sealed class GetCategoryByIdQueryValidator : AbstractValidator<GetCategoryByIdQuery>
{
    public GetCategoryByIdQueryValidator()
    {
        RuleFor(x => x.categoryId)
            .NotEmpty()
            .WithMessage("CategoryId is required")
            .Must(a => Ulid.TryParse(a, out _))
            .WithMessage("CategoryId is not in the correct format");
    }
}
