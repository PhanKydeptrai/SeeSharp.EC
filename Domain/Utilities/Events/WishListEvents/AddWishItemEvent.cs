namespace Domain.Utilities.Events.WishListEvents;

public record AddWishItemEvent(
    Guid WishItemId,
    Guid ProductId, 
    Guid CustomerId,
    Guid MessageId);
