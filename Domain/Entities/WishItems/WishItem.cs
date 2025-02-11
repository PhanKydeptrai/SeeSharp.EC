using Domain.Entities.Customers;
using Domain.Entities.Products;

namespace Domain.Entities.WishItems;

public sealed class WishItem
{
    public WishItemId WishItemId { get; private set; } = null!; 
    public CustomerId CustomerId { get; private set; } = null!;
    public ProductId ProductId { get; private set; } = null!;

    //Foreign key
    public Customer? Customer { get; set; } = null!;
    public Product? Product { get; set; } = null!;


    private WishItem(
        WishItemId wishItemId, 
        CustomerId customerId, 
        ProductId productId)
    {
        WishItemId = wishItemId;
        CustomerId = customerId;
        ProductId = productId;
    }

    public static WishItem NewWishItem(
        WishItemId wishItemId,
        CustomerId customerId,
        ProductId productId)
    {
        return new WishItem(wishItemId, customerId, productId);
    }
}
