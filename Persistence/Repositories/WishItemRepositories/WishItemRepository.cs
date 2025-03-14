using Domain.Entities.WishItems;
using Domain.IRepositories.WishItems;
using Persistence.Database.MySQL;
using Persistence.Database.PostgreSQL;

namespace Persistence.Repositories.WishItemRepositories;

internal sealed class WishItemRepository : IWishItemRepository
{
    private readonly NextSharpMySQLWriteDbContext _mysqlWriteDbContext;
    private readonly NextSharpPostgreSQLWriteDbContext _postgreSQLWriteDbContext;

    public WishItemRepository(
        NextSharpMySQLWriteDbContext mysqlWriteDbContext, 
        NextSharpPostgreSQLWriteDbContext postgreSQLWriteDbContext)
    {
        _mysqlWriteDbContext = mysqlWriteDbContext;
        _postgreSQLWriteDbContext = postgreSQLWriteDbContext;
    }

    public async Task AddWishItemToMySQL(WishItem wishItem)
    {
        await _mysqlWriteDbContext.WishItems.AddAsync(wishItem);
    }

    public async Task AddWishItemToPostgreSQL(WishItem wishItem)
    {
        await _postgreSQLWriteDbContext.WishItems.AddAsync(wishItem);
    }
}
