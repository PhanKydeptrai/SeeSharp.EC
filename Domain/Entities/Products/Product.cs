using Domain.Entities.Categories;

namespace Domain.Entities.Products;

public sealed class Product
{
    public ProductId ProductId { get; private set; } = null!;
    public ProductName ProductName { get; private set; } = ProductName.Empty;
    public string? ImageUrl { get; private set; }
    public string? Description { get; private set; }
    public ProductPrice ProductPrice { get; private set; } = null!;
    public ProductStatus ProductStatus { get; private set; } 
    public CategoryId CategoryId { get; private set; }
    public Category? Category { get; private set; } = null!;
    //* Foreign key
    // public ICollection<WishItem>? WishItems { get; private set; } = null!; //FIXME: Change to WishItems
    // public ICollection<OrderDetail>? OrderDetails { get; private set; } = null!; //FIXME: Change to OrderDetails

    private Product(
        ProductId productId, 
        ProductName productName, 
        string imageUrl, 
        string description, 
        ProductPrice productPrice, 
        ProductStatus productStatus,
        CategoryId categoryId)
    {
        ProductId = productId;
        ProductName = productName;
        ImageUrl = imageUrl;
        Description = description;
        ProductPrice = productPrice;
        ProductStatus = productStatus;
        CategoryId = categoryId;
    }

    public static Product NewProduct(
        ProductId productId, 
        ProductName productName, 
        string? imageUrl, 
        string? description, 
        ProductPrice productPrice,
        CategoryId categoryId)
    {
        return new Product(
            productId, 
            productName, 
            imageUrl ?? string.Empty, 
            description ?? string.Empty, 
            productPrice, 
            ProductStatus.InStock, 
            categoryId);
    }
}

