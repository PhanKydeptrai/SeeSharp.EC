using Domain.Entities.Products;

namespace Domain.Entities.Categories;

public sealed class Category
{
    public CategoryId CategoryId { get; private set; }
    public CategoryName CategoryName { get; private set; } = CategoryName.Empty;
    public string? ImageUrl { get; private set; }
    public CategoryStatus CategoryStatus { get; private set; }
    public bool IsDefault { get; private set; }
    public ICollection<Product>? Products { get; set; }

    private Category(
        CategoryId categoryId,
        CategoryName categoryName,
        string imageUrl,
        CategoryStatus categoryStatus,
        bool isDefault)
    {
        CategoryId = categoryId;
        CategoryName = categoryName;
        ImageUrl = imageUrl;
        CategoryStatus = categoryStatus;
        IsDefault = isDefault;
    }

    public static Category NewCategory(
        CategoryName categoryName,
        string? imageUrl)
    {
        return new Category(
            CategoryId.New(),
            categoryName,
            imageUrl ?? string.Empty,
            CategoryStatus.Available,
            false);
    }

    public void Update(
        CategoryName categoryName,
        CategoryStatus categoryStatus,
        string imageUrl)
    {
        CategoryName = categoryName;
        CategoryStatus = categoryStatus;
        ImageUrl = imageUrl;
    }

    public void Restore()
    {
        if (CategoryStatus != CategoryStatus.Deleted)
        {
            throw new InvalidOperationException("Category is not deleted");
        }
        CategoryStatus = CategoryStatus.Available;
    }
    public void Delete()
    {
        if (CategoryStatus == CategoryStatus.Deleted)
        {
            throw new InvalidOperationException("Category is already deleted");
        }
        CategoryStatus = CategoryStatus.Deleted;
    }
    public static Category FromExisting(
        CategoryId categoryId,
        CategoryName categoryName,
        string? imageUrl,
        CategoryStatus categoryStatus,
        bool isDefault)
    {
        return new Category(
            categoryId,
            categoryName,
            imageUrl ?? string.Empty,
            categoryStatus,
            isDefault);
    }
}
