using Domain.IRepositories;
using Domain.IRepositories.CategoryRepositories;
using Domain.Utilities.Events.CategoryEvents;
using MassTransit;

namespace Application.Consumers.Category;

internal sealed class CategoryCreatedEventConsumer : IConsumer<CategoryCreatedEvent>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryCreatedEventConsumer(
        ICategoryRepository categoryRepository, 
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<CategoryCreatedEvent> context)
    {
        await _categoryRepository.AddCategoryToPosgreSQL(context.Message.category);

        var result = await _unitOfWork.SaveChangesAsync();

        if (result <= 0)
        {
            throw new Exception("Category created event consumed failed");
        }
    }
}
