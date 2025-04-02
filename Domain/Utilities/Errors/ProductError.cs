using Domain.Entities.Products;
using Domain.Entities.ProductVariants;
using SharedKernel;

namespace Domain.Utilities.Errors;

public static class ProductError
{
    public static Error ProductNotFound(ProductId ProductId) => Error.NotFound(
        "Product.NotFound",
        $"The Product with the Id = '{ProductId}' was not found");
    public static Error VariantNotFound(ProductVariantId ProductVariantId) => Error.NotFound(
        "ProductVariant.NotFound",
        $"The Product variant with the Id = '{ProductVariantId}' was not found");
    public static Error Problem(ProductId ProductId) => Error.Problem(
        "Product.Problem",
        $"Cant save Product with the Id = '{ProductId}'");

    public static Error Failure(ProductId ProductId) => Error.Failure(
        "Product.Failure", 
        $"Failed to update Product with the Id = '{ProductId}'");

    public static Error Deleted(ProductId ProductId) => Error.Problem(
        "Product.Deleted",
        $"The Product with the Id = '{ProductId}' was deleted");
    
    public static Error ProductNotDiscontinued(ProductId ProductId) => Error.Problem(
        "Product.NotDiscontinued",
        $"The Product with the Id = '{ProductId}' is not discontinued");

    public static Error VariantNotDiscontinued(ProductVariantId VariantProductId) => Error.Problem(
        "Variant.NotDiscontinued",
        $"The Product Variant with the Id = '{VariantProductId}' is not discontinued");
}
