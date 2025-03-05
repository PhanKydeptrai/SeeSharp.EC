using Domain.Entities.UserAuthenticationTokens;
using Domain.IRepositories.UserAuthenticationTokens;
using Persistence.Database.MySQL;

namespace Persistence.Repositories.UserAuthenticationTokenRepositories;

internal sealed class UserAuthenticationTokenRepository : IUserAuthenticationTokenRepository
{
    private readonly NextSharpMySQLWriteDbContext _nextSharpMySQLWriteDbContext;
    public UserAuthenticationTokenRepository(
        NextSharpMySQLWriteDbContext nextSharpMySQLWriteDbContext)
    {
        _nextSharpMySQLWriteDbContext = nextSharpMySQLWriteDbContext;
    }

    public async Task AddUserAuthenticationTokenToMySQL(
        UserAuthenticationToken accessToken, 
        UserAuthenticationToken refreshToken)
    {
        await _nextSharpMySQLWriteDbContext.UserAuthenticationTokens.AddRangeAsync(accessToken, refreshToken);
    }
}
