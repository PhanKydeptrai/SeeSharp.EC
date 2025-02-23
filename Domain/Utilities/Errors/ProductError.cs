using Domain.Entities.Products;
using SharedKernel;

namespace Domain.Utilities.Errors;

public static class ProductError
{
    public static Error NotFound(ProductId ProductId) => Error.NotFound(
        "Product.NotFound",
        $"The Product with the Id = '{ProductId}' was not found");
    public static Error Problem(ProductId ProductId) => Error.Problem(
        "Product.Problem",
        $"Cant save Product with the Id = '{ProductId}'");

    public static Error Failure(ProductId ProductId) => Error.Failure(
        "Product.Conflict",
        $"Failed to update Product with the Id = '{ProductId}'");

    public static Error Deleted(ProductId ProductId) => Error.Failure(
        "Product.Deleted",
        $"The Product with the Id = '{ProductId}' was deleted");

}
