using Application.Abstractions.Messaging;
using Domain.Entities.Categories;
using Domain.IRepositories;
using Domain.IRepositories.CategoryRepositories;
using SharedKernel;

namespace Application.Features.CategoryFeature.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        
        var category = Category.FromExisting(
            CategoryId.FromString(request.categoryId),
            CategoryName.FromString(request.categoryName),
            string.Empty);
        
        await _categoryRepository.UpdateCategoryToMySQL(category);

        return Result.Success();
    }
}
