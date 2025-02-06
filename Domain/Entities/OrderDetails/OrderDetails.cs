namespace Domain.Entities.OrderDetails;

public sealed class OrderDetails
{
    public Ulid OrderDetailId { get; set; }
    public Ulid OrderId { get; set; }
    public Ulid MealId { get; set; }
    public int Quantity { get; set; }
    public string? Note { get; set; }
    public decimal UnitPrice { get; set; }
    // public Order? Order { get; set; } //FIXME:
    // public Meal? Meal { get; set; } //FIXME:
}
