using Domain.Entities.WishItems;

namespace Domain.IRepositories.WishItems;

public interface IWishItemRepository
{
    Task AddWishItemToMySQL(WishItem wishItem);
    Task AddWishItemToPostgreSQL(WishItem wishItem);
    Task<WishItem?> GetWishItemByIdFromMySQL(WishItemId wishItemId);
    Task<WishItem?> GetWishItemByIdFromPostgreSQL(WishItemId wishItemId);
    void DeleteWishItemFromMySQL(WishItem wishItem);
    void DeleteWishItemFromPostgreSQL(WishItem wishItem);
}
