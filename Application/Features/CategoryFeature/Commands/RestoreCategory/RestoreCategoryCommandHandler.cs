using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Application.Outbox;
using Domain.Entities.Categories;
using Domain.IRepositories;
using Domain.IRepositories.Products;
using Domain.IRepositories.CategoryRepositories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Errors;
using Domain.Utilities.Events.CategoryEvents;
using SharedKernel;

namespace Application.Features.CategoryFeature.Commands.RestoreCategory;

public class RestoreCategoryCommandHandler : ICommandHandler<RestoreCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IEventBus _eventBus;
    public RestoreCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ICategoryRepository categoryRepository,
        IProductRepository productRepository,
        IEventBus eventBus,
        IOutBoxMessageServices outBoxMessageServices)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _eventBus = eventBus;
        _outBoxMessageServices = outBoxMessageServices;
    }

    public async Task<Result> Handle(RestoreCategoryCommand request, CancellationToken cancellationToken)
    {
        //Start transaction
        using var transaction = await _unitOfWork.BeginPostgreSQLTransaction();
        //Process restore category
        var categoryId = CategoryId.FromGuid(request.CategoryId);
        var (category, failure) = await GetCategoryById(categoryId);
        if(category is null)
        {
            transaction.Rollback(); //Close transaction if category is null
            return failure!;
        }
        category.Restore();
        //Insert outbox message
        await _unitOfWork.SaveChangeAsync();
        
        //Process product restore by category
        await _productRepository.RestoreProductByCategoryFromPostgreSQL(categoryId);
        //Commit transaction
        transaction.Commit();
        
        return Result.Success();
    }

    private async Task<(Category? category, Result? result)> GetCategoryById(CategoryId categoryId)
    {
        var category = await _categoryRepository.GetCategoryByIdFromMySQL(categoryId);
        if (category is null)
        {
            return (null, Result.Failure(CategoryErrors.NotFound(categoryId)));
        }
        if(category.CategoryStatus != CategoryStatus.Deleted)
        {
            return (null, Result.Failure(CategoryErrors.NotDeleted(categoryId)));
        }

        return (category, null);
    }
}
