using Domain.Entities.Categories;
using SharedKernel;

namespace Domain.Utilities.Errors;

public static class CategoryErrors 
{
    public static Error NotFound(CategoryId categoryId) => Error.NotFound(
        "Category.NotFound",
        $"The category with the Id = '{categoryId}' was not found");

    public static Error Failure(CategoryId categoryId) => Error.Failure(
        "Category.Failure",
        $"Cant save category with the Id = '{categoryId}'"); //Lỗi hết cứu

    public static Error NameConflict(CategoryName categoryName) => Error.Conflict(
        "CategoryName.Conflict",
        $"Failed to update category with the Id = '{categoryName.Value}'");
    
    public static Error Deleted(CategoryId categoryId) => Error.Problem(
        "Category.Deleted",
        $"The category with the Id = '{categoryId}' was deleted");
}
