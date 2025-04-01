using Application.Abstractions.Messaging;
using Domain.Entities.Categories;
using Domain.IRepositories;
using Domain.IRepositories.CategoryRepositories;
using SharedKernel;

namespace Application.Features.CategoryFeature.Commands.CreateCategory;

internal class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CategoryId>
{
    #region Dependency
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    } 
    #endregion
    //FLOW: Create category -> Add category to Postgres
    public async Task<Result<CategoryId>> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        //TODO: Xử lý ảnh
        //--------------------

        string imageUrl = string.Empty;
        //--------------------

        var category = Category.NewCategory(
            CategoryName.NewCategoryName(request.categoryName),
            imageUrl);

        await _categoryRepository.AddCategoryToPosgreSQL(category);
        await _unitOfWork.SaveChangeAsync();
        return Result.Success(category.CategoryId);
    }
}
