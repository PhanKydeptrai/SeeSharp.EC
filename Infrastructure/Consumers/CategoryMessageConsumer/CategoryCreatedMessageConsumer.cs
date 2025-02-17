using Domain.Entities.Categories;
using Domain.IRepositories;
using Domain.IRepositories.CategoryRepositories;
using Domain.Utilities.Events.CategoryEvents;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Consumers.CategoryMessageConsumer;

internal sealed class CategoryCreatedMessageConsumer : IConsumer<CategoryCreatedEvent>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoryCreatedMessageConsumer> _logger;
    public CategoryCreatedMessageConsumer(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        ILogger<CategoryCreatedMessageConsumer> logger)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<CategoryCreatedEvent> context)
    {
        _logger.LogInformation(
            "Consuming CategoryCreatedEvent for categoryId: {CategoryId}", 
            context.Message.categoryId);

        var category = ConvertEventToCategory(context.Message);

        await _categoryRepository.AddCategoryToPosgreSQL(category);
        var result = await _unitOfWork.SaveChangesAsync();

        if (result <= 0)
        {
            _logger.LogError(
                "Failed to consume CategoryCreatedEvent for categoryId: {CategoryId}", 
                context.Message.categoryId);

            throw new Exception("Category created event consumed failed");
        }

        _logger.LogInformation(
            "Successfully consumed CategoryCreatedEvent for categoryId: {CategoryId}", 
            context.Message.categoryId);
    }

    private Category ConvertEventToCategory(CategoryCreatedEvent categoryCreatedEvent)
    {
        return Category.FromExisting(
            CategoryId.FromUlid(categoryCreatedEvent.categoryId),
            CategoryName.NewCategoryName(categoryCreatedEvent.categoryName),
            categoryCreatedEvent.imageUrl
        );
    }   
}
