using Domain.Entities.WishItems;

namespace Domain.IRepositories.WishItems;

public interface IWishItemRepository
{
    Task AddWishItemToPostgreSQL(WishItem wishItem);
    Task<WishItem?> GetWishItemByIdFromPostgreSQL(WishItemId wishItemId);
    void DeleteWishItemFromPostgreSQL(WishItem wishItem);
}
