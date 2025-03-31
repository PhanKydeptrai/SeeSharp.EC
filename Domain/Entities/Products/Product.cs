using Domain.Entities.Categories;
using Domain.Entities.OrderDetails;
using Domain.Entities.ProductVariants;
using Domain.Entities.WishItems;

namespace Domain.Entities.Products;

public sealed class Product
{
    public ProductId ProductId { get; private set; } = null!;
    public ProductName ProductName { get; private set; } = ProductName.Empty;
    public string? ImageUrl { get; private set; }
    public string? Description { get; private set; }
    public ProductStatus ProductStatus { get; private set; } 
    public CategoryId CategoryId { get; private set; }
    public Category? Category { get; private set; } = null!;
    //* Foreign keys
    public ICollection<ProductVariant>? ProductVariants { get; private set; } = null!;
    private Product(
        ProductId productId, 
        ProductName productName, 
        string imageUrl, 
        string description,
        ProductStatus productStatus,
        CategoryId categoryId)
    {
        ProductId = productId;
        ProductName = productName;
        ImageUrl = imageUrl;
        Description = description;
        ProductStatus = productStatus;
        CategoryId = categoryId;
    }

    public static Product FromExisting(
        ProductId productId,
        ProductName productName,
        string? imageUrl,
        string? description,
        ProductStatus productStatus,
        CategoryId categoryId)
    {
        return new Product(
            productId,
            productName,
            imageUrl ?? string.Empty,
            description ?? string.Empty,
            productStatus,
            categoryId);
    }
    public static Product NewProduct( 
        ProductName productName, 
        string? imageUrl, 
        string? description,
        CategoryId? categoryId)
    {
        return new Product(
            ProductId.New(), 
            productName, 
            imageUrl ?? string.Empty, 
            description ?? string.Empty, 
            ProductStatus.InStock, 
            categoryId ?? CategoryId.DefaultCategoryId);
    }

    public void Update(
        ProductName productName,
        string? imageUrl,
        string? description,
        ProductStatus productStatus,
        CategoryId categoryId)
    {
        ProductName = productName;
        ImageUrl = imageUrl;
        Description = description;
        ProductStatus = productStatus;
        CategoryId = categoryId;
    }

    public void Delete()
    {
        if(ProductStatus == ProductStatus.Discontinued)
        {
            throw new InvalidOperationException("Product is already deleted");
        }
        
        ProductStatus = ProductStatus.Discontinued;
    }

    public void Restore()
    {
        if(this.ProductStatus == ProductStatus.InStock)
        {
            throw new InvalidOperationException("Product is already in stock");
        }
        
        this.ProductStatus = ProductStatus.InStock;
    }
}

