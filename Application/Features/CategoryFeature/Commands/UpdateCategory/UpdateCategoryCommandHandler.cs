using Application.Abstractions.Messaging;
using Domain.Entities.Categories;
using Domain.IRepositories.CategoryRepositories;
using Domain.Utilities.Errors;
using Domain.Utilities.Events.CategoryEvents;
using MediatR;
using SharedKernel;

namespace Application.Features.CategoryFeature.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IPublisher _publisher;
    public UpdateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IPublisher publisher)
    {
        _categoryRepository = categoryRepository;
        _publisher = publisher;
    }

    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = Category.FromExisting(
            CategoryId.FromString(request.categoryId),
            CategoryName.FromString(request.categoryName),
            string.Empty);

        var result = await _categoryRepository.UpdateCategoryToMySQL(category);

        if (result > 0)
        {
            await _publisher.Publish(
                new CategoryUpdatedEvent(
                    category.CategoryId.Value,
                    category.CategoryName.Value,
                    category.ImageUrl ?? string.Empty,
                    Ulid.NewUlid()),
                cancellationToken);

            return Result.Success();
        }
        return Result.Failure(CategoryErrors.Failure(category.CategoryId));
    }
}
