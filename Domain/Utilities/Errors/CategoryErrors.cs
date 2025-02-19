using Domain.Entities.Categories;
using SharedKernel;

namespace Domain.Utilities.Errors;

public static class CategoryErrors 
{
    public static Error NotFound(CategoryId categoryId) => Error.NotFound(
        "Category.NotFound",
        $"The category with the Id = '{categoryId}' was not found");

    public static Error Problem(CategoryId categoryId) => Error.Problem(
        "Category.Problem",
        $"Cant save category with the Id = '{categoryId}'");

    public static Error Failure(CategoryId categoryId) => Error.Failure(
        "Category.Conflict",
        $"Failed to update category with the Id = '{categoryId}'");
}
