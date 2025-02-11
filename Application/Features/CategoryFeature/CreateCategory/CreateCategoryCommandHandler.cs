using Application.Abstractions.Messaging;
using Domain.Entities.Categories;
using Domain.IRepositories.CategoryRepositories;
using SharedKernel;

namespace Application.Features.CategoryFeature.CreateCategory;

internal class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        //FIXME: Xử lý ảnh
        string imageUrl = string.Empty;

        await _categoryRepository.AddCategoryToMySQL(
            Category.NewCategory(CategoryName.NewCategoryName(request.categoryName), imageUrl));

        return Result.Success();
    }
}
