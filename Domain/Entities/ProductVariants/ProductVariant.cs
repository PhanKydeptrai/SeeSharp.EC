using Domain.Entities.OrderDetails;
using Domain.Entities.Products;
using Domain.Entities.WishItems;

namespace Domain.Entities.ProductVariants;

public sealed class ProductVariant
{
    public ProductVariantId ProductVariantId { get; private set; } = null!;
    public VariantName VariantName { get; private set; } = VariantName.Empty;
    public ProductVariantPrice ProductVariantPrice { get; private set; } = null!;
    public ColorCode ColorCode { get; private set; } = null!;
    public string? ImageUrl { get; private set; }
    public ProductVariantDescription Description { get; private set; } = ProductVariantDescription.Empty;
    public ProductId ProductId { get; private set; } = null!;
    public ProductVariantStatus ProductVariantStatus { get; private set; }
    public IsBaseVariant IsBaseVariant { get; private set; } = null!;
    public Product? Product { get; set; } = null!;
    public ICollection<WishItem>? WishItems { get; set; } = null!;
    public ICollection<OrderDetail>? OrderDetails { get; set; } = null!;
    private ProductVariant(
        ProductVariantId productVariantId, 
        VariantName variantName, 
        ProductVariantPrice productVariantPrice,
        ColorCode colorCode,
        string? imageUrl,
        ProductVariantDescription description, 
        ProductId productId,
        IsBaseVariant isBaseVariant)
    {
        ProductVariantId = productVariantId;
        VariantName = variantName;
        ProductVariantPrice = productVariantPrice;
        ColorCode = colorCode;
        ImageUrl = imageUrl;
        Description = description;
        ProductId = productId;
        IsBaseVariant = isBaseVariant;
    }

    //* Factory methods
    public static ProductVariant Create(
        VariantName variantName,
        ProductVariantPrice productVariantPrice,
        ColorCode ColorCode,
        ProductVariantDescription productVariantDescription,
        ProductId productId, 
        string? imageUrl,
        IsBaseVariant isBaseVariant)
    {
        return new ProductVariant(
            ProductVariantId.New(),
            variantName,
            productVariantPrice,
            ColorCode,
            imageUrl ?? string.Empty,
            productVariantDescription,
            productId,
            isBaseVariant);
    }

    public void Update(
        VariantName variantName,
        ProductVariantPrice productVariantPrice,
        ColorCode colorCode,
        ProductVariantDescription productVariantDescription,
        string? imageUrl,
        IsBaseVariant isBaseVariant)
    {
        VariantName = variantName;
        ProductVariantPrice = productVariantPrice;
        ColorCode = colorCode;
        ImageUrl = imageUrl;
        Description = productVariantDescription;
        IsBaseVariant = isBaseVariant;
    }

    public void Delete()
    {
        ProductVariantStatus = ProductVariantStatus.Discontinued;
    }

    public void IsNotBase()
    {
        IsBaseVariant = IsBaseVariant.False;
    }

    public void IsBase()
    {
        IsBaseVariant = IsBaseVariant.True;
    }

    public void Restore()
    {
        ProductVariantStatus = ProductVariantStatus.InStock;
    }
}
