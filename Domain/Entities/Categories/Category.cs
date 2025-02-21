using Domain.Entities.Products;

namespace Domain.Entities.Categories;

public sealed class Category
{
    public CategoryId CategoryId { get; private set; }
    public CategoryName CategoryName { get; set; } = CategoryName.Empty;
    public string? ImageUrl { get; set; }
    public CategoryStatus CategoryStatus { get; set; }
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
        string imageUrl)
    {
        category.CategoryName = categoryName;
        category.ImageUrl = imageUrl;
    }

    public static Category FromExisting(
        CategoryId categoryId,
        CategoryName categoryName,
        string? imageUrl)
    {
        return new Category(
            categoryId,
            categoryName,
            imageUrl ?? string.Empty,
            CategoryStatus.Available);
    }
}
