using Domain.Entities.Products;

namespace Domain.Entities.Categories;

public sealed class Category
{
    public CategoryId CategoryId { get; private set; }
    public CategoryName CategoryName { get; private set; } = CategoryName.Empty;
    public string? ImageUrl { get; private set; }
    public CategoryStatus CategoryStatus { get; private set; }
    public ICollection<Product>? Products { get; set; }

    private Category(
        CategoryId categoryId, 
        CategoryName categoryName, 
        string imageUrl, 
        CategoryStatus categoryStatus)
    {
        CategoryId = categoryId;
        CategoryName = categoryName;
        ImageUrl = imageUrl;
        CategoryStatus = categoryStatus;
    }

    public static Category NewCategory(
        CategoryName categoryName, 
        string? imageUrl)
    {
        return new Category(
            CategoryId.New(), 
            categoryName, 
            imageUrl ?? string.Empty, 
            CategoryStatus.Available);
    }

    public static void Update(
        Category category, 
        CategoryName categoryName,
        CategoryStatus categoryStatus,
        string imageUrl)
    {
        category.CategoryName = categoryName;
        category.CategoryStatus = categoryStatus;
        category.ImageUrl = imageUrl;
    }

    public static void Delete(Category category)
    {
        category.CategoryStatus = CategoryStatus.Deleted;
    }

    public static Category FromExisting(
        CategoryId categoryId,
        CategoryName categoryName,
        string? imageUrl,
        CategoryStatus categoryStatus)
    {
        return new Category(
            categoryId,
            categoryName,
            imageUrl ?? string.Empty,
            categoryStatus);
    }
}
