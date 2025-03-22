using Domain.Entities.WishItems;
using SharedKernel;

namespace Domain.Utilities.Errors;

public static class WishItemError 
{
    public static Error NotFound(WishItemId wishItemId) => Error.NotFound(
        "WishItem.NotFound",
        $"The Wish Item with the Id = '{wishItemId}' was not found");
}
