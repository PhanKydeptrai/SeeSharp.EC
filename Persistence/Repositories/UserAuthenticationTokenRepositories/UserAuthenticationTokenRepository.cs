using Domain.Entities.UserAuthenticationTokens;
using Domain.Entities.Users;
using Domain.IRepositories.UserAuthenticationTokens;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL;

namespace Persistence.Repositories.UserAuthenticationTokenRepositories;

internal sealed class UserAuthenticationTokenRepository : IUserAuthenticationTokenRepository
{
    private readonly SeeSharpPostgreSQLWriteDbContext _context;
    public UserAuthenticationTokenRepository(SeeSharpPostgreSQLWriteDbContext context)
    {
        _context = context;
    }

    public async Task AddRefreshToken(UserAuthenticationToken refreshToken)
    {
        await _context.UserAuthenticationTokens.AddAsync(refreshToken);
    }
    public async Task<UserAuthenticationToken?> GetRefreshTokenFromMySQLByJti(string jti)
    {
        return await _context.UserAuthenticationTokens
            .Where(x => x.Jti == jti)
            .FirstOrDefaultAsync();
    }

    public async Task<UserAuthenticationToken?> GetAuthenticationTokenWithRefreshToken(string refreshToken)
    {
        var userAuthenticationToken = await _context.UserAuthenticationTokens
            .Where(x => x.Value == refreshToken)
            .Include(x => x.User)
            .Include(x => x.User!.Customer)
            .FirstOrDefaultAsync();

        return userAuthenticationToken;
    }

    public async Task RevokeAllTokenFromMySQLByUserId(UserId userId)
    {
        await _context.UserAuthenticationTokens.Where(x => x.UserId == userId)
            .ExecuteUpdateAsync(a => a.SetProperty(a => a.IsBlackList, IsBlackList.True));
    }
}
