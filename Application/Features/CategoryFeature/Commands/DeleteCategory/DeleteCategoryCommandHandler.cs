using Application.Abstractions.Messaging;
using Domain.Entities.Categories;
using Domain.IRepositories;
using Domain.IRepositories.Products;
using Domain.IRepositories.CategoryRepositories;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.CategoryFeature.Commands.DeleteCategory;

internal sealed class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand>
{
   
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        IProductRepository productRepository)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
    }

    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        //Start transaction
        using var transaction = await _unitOfWork.BeginPostgreSQLTransaction();
        var categoryId = CategoryId.FromGuid(request.categoryId);

        //Process delete category
        var (category, failure) = await GetCategoryByIdAsync(CategoryId.FromGuid(categoryId));
        if (category is null)
        {
            transaction.Rollback();
            return failure!;
        }
        category.Delete();

        await _unitOfWork.SaveChangeAsync();

        //Delete all products in this category
        await _productRepository.DeleteProductByCategory(category.CategoryId);
        //Delete all variant of products in this category
        await _productRepository.DeleteProductVariantByCategory(category.CategoryId);
        transaction.Commit();
        return Result.Success();
    }
    //----------------------------------------------------------------
    private async Task<(Category? category, Result? failure)> GetCategoryByIdAsync(CategoryId categoryId)
    {
        
        var category = await _categoryRepository.GetCategoryByIdFromPostgreSQL(categoryId);

        if (category is null)
        {
            return (null, Result.Failure(CategoryErrors.NotFound(categoryId)));
        }

        if (category.IsDefault)
        {
            return (null, Result.Failure(CategoryErrors.IsDefault(categoryId)));
        }

        if (category.CategoryStatus == CategoryStatus.Deleted)
        {
            return (null, Result.Failure(CategoryErrors.Deleted(categoryId)));
        }

        return (category, null);
    }
}