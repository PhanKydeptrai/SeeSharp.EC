using Domain.Entities.WishItems;

namespace Domain.IRepositories.WishItems;

public interface IWishItemRepository
{
    Task AddWishItemToMySQL(WishItem wishItem);
    Task AddWishItemToPostgreSQL(WishItem wishItem);
}
