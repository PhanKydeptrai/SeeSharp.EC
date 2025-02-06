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
}
