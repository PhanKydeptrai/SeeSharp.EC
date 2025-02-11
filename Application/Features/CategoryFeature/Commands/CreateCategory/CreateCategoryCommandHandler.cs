using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Domain.Entities.Categories;
using Domain.IRepositories;
using Domain.IRepositories.CategoryRepositories;
using Domain.Utilities.Errors;
using Domain.Utilities.Events.CategoryEvents;
using SharedKernel;

namespace Application.Features.CategoryFeature.Commands.CreateCategory;

internal class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;
    public CreateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        IEventBus eventBus)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
    }

    public async Task<Result> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
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
            await _eventBus.PublishAsync(
                new CategoryCreatedEvent(category), 
                cancellationToken);

            return Result.Success();
        }

        return Result.Failure(CategoryErrors.Problem(category.CategoryId));
    }
}
