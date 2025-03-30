using Domain.Entities.WishItems;
using Domain.IRepositories.WishItems;
using Persistence.Database.PostgreSQL;

namespace Persistence.Repositories.WishItemRepositories;

internal sealed class WishItemRepository : IWishItemRepository
{
    private readonly SeeSharpPostgreSQLWriteDbContext _postgreSQLWriteDbContext;

    public WishItemRepository(
        SeeSharpPostgreSQLWriteDbContext postgreSQLWriteDbContext)
    {
        _postgreSQLWriteDbContext = postgreSQLWriteDbContext;
    }


    public async Task AddWishItemToPostgreSQL(WishItem wishItem)
    {
        await _postgreSQLWriteDbContext.WishItems.AddAsync(wishItem);
    }

    public async Task<WishItem?> GetWishItemByIdFromPostgreSQL(WishItemId wishItemId)
    {
        return await _postgreSQLWriteDbContext.WishItems.FindAsync(wishItemId);
    }


    public void DeleteWishItemFromPostgreSQL(WishItem wishItem)
    {
        _postgreSQLWriteDbContext.WishItems.Remove(wishItem);
    }
}
