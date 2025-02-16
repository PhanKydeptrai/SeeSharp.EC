using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Domain.Entities.Categories;
using Domain.IRepositories;
using Domain.IRepositories.CategoryRepositories;
using Domain.Utilities.Errors;
using Domain.Utilities.Events.CategoryEvents;
using MediatR;
using SharedKernel;

namespace Application.Features.CategoryFeature.Commands.CreateCategory;

internal class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher; //MediatR Notification

    public CreateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        IPublisher publisher)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task<Result> Handle(
        CreateCategoryCommand request, 
        CancellationToken cancellationToken)
    {
        //FIXME: Xử lý ảnh
        string imageUrl = string.Empty;

        var category = Category.NewCategory(
            CategoryName.NewCategoryName(request.categoryName),
            imageUrl);

        await _categoryRepository.AddCategoryToMySQL(category);

        int result = await _unitOfWork.Commit(cancellationToken);

        if (result > 0)
        {
            await _publisher.Publish(
                new CategoryCreatedEvent(
                    category.CategoryId.Value,
                    category.CategoryName.Value,
                    category.ImageUrl ?? string.Empty),
                cancellationToken);

            //await _eventBus.PublishAsync(
            //    new CategoryCreatedEvent(
            //        category.CategoryId.Value, 
            //        category.CategoryName.Value, 
            //        category.ImageUrl ?? string.Empty),
            //    cancellationToken);

            return Result.Success();
        }

        return Result.Failure(CategoryErrors.Problem(category.CategoryId));
    }
}
