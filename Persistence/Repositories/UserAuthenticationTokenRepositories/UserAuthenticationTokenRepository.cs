using Domain.Entities.UserAuthenticationTokens;
using Domain.Entities.Users;
using Domain.IRepositories.UserAuthenticationTokens;
using Microsoft.EntityFrameworkCore;
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

    public async Task AddRefreshTokenToMySQL(UserAuthenticationToken refreshToken)
    {
        await _nextSharpMySQLWriteDbContext.UserAuthenticationTokens.AddAsync(refreshToken);
    }
    public async Task<UserAuthenticationToken?> GetRefreshTokenFromMySQLByUserId(UserId userId)
    {
        return await _nextSharpMySQLWriteDbContext.UserAuthenticationTokens
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<UserAuthenticationToken?> GetAuthenticationTokenWithRefreshToken(string refreshToken)
    {
        var userAuthenticationToken = await _nextSharpMySQLWriteDbContext.UserAuthenticationTokens
            .Where(x => x.Value == refreshToken)
            .Include(x => x.User)
            .Include(x => x.User!.Customer)
            .FirstOrDefaultAsync();
        
        return userAuthenticationToken;
    }
}
