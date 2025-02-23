using FluentValidation;

namespace Application.Features.ProductFeature.Commands.CreateProduct;

internal sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.ProductName)
            .NotEmpty()
            .WithErrorCode("ProductName.IsRequied")
            .WithMessage("Product name is required")
            .MaximumLength(50)
            .WithErrorCode("ProductName.TooLong")
            .WithMessage("Product name is too long");

        RuleFor(x => x.Price)
            .NotEmpty()
            .WithErrorCode("Price.IsRequied")
            .WithMessage("Price is required")
            .GreaterThan(0)
            .WithErrorCode("Price.MustGreaterThanZero")
            .WithMessage("Price is lower than 0");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithErrorCode("CategoryId.IsRequied")
            .WithMessage("Category id is required")
            .Must(a => Ulid.TryParse(a, out _))
            .WithMessage("CategoryId is not in the correct format");

    }
}
