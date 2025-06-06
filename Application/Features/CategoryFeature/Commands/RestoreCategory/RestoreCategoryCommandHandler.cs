using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Domain.Entities.Categories;
using Domain.IRepositories;
using Domain.IRepositories.Products;
using Domain.IRepositories.CategoryRepositories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.CategoryFeature.Commands.RestoreCategory;

public class RestoreCategoryCommandHandler : ICommandHandler<RestoreCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    public RestoreCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ICategoryRepository categoryRepository,
        IProductRepository productRepository)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
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
        await _unitOfWork.SaveChangesAsync();
        
        //Process product restore by category
        await _productRepository.RestoreProductByCategory(categoryId);
        await _productRepository.RestoreProductVariantByCategory(categoryId);
        //Commit transaction
        transaction.Commit();
        
        return Result.Success();
    }

    private async Task<(Category? category, Result? result)> GetCategoryById(CategoryId categoryId)
    {
        
        var category = await _categoryRepository.GetCategoryByIdFromPostgreSQL(categoryId);
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
