using Domain.Entities.Customers;
using Domain.Entities.ProductVariants;

namespace Domain.Entities.WishItems;

public sealed class WishItem
{
    public WishItemId WishItemId { get; private set; } = null!; 
    public CustomerId CustomerId { get; private set; } = null!;
    public ProductVariantId ProductVariantId { get; private set; } = null!;

    //Foreign key
    public Customer? Customer { get; set; } = null!;
    public ProductVariant? ProductVariant { get; set; } = null!;


    private WishItem(
        WishItemId wishItemId, 
        CustomerId customerId, 
        ProductVariantId productVariantId)
    {
        WishItemId = wishItemId;
        CustomerId = customerId;
        ProductVariantId = productVariantId;
    }

    public static WishItem NewWishItem(
        CustomerId customerId,
        ProductVariantId productVariantId)
    {
        return new WishItem(
            WishItemId.New(), 
            customerId, 
            productVariantId);
    }

    public static WishItem FromExisting(
        WishItemId wishItemId,
        CustomerId customerId,
        ProductVariantId productVariantId)
    {
        return new WishItem(
            wishItemId, 
            customerId, 
            productVariantId);
    }
}
