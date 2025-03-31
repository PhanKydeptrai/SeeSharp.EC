//using Domain.Entities.UserAuthenticationTokens;
//using Domain.Entities.Users;
//using Domain.IRepositories.UserAuthenticationTokens;
//using Microsoft.EntityFrameworkCore;

//namespace Persistence.Repositories.UserAuthenticationTokenRepositories;

//internal sealed class UserAuthenticationTokenRepository : IUserAuthenticationTokenRepository
//{
//    public UserAuthenticationTokenRepository(NextSharpMySQLWriteDbContext nextSharpMySQLWriteDbContext)
//    {
//        _nextSharpMySQLWriteDbContext = nextSharpMySQLWriteDbContext;
//    }

//    public async Task AddRefreshTokenToMySQL(UserAuthenticationToken refreshToken)
//    {
//        await _nextSharpMySQLWriteDbContext.UserAuthenticationTokens.AddAsync(refreshToken);
//    }
//    public async Task<UserAuthenticationToken?> GetRefreshTokenFromMySQLByJti(string jti)
//    {
//        return await _nextSharpMySQLWriteDbContext.UserAuthenticationTokens
//            .Where(x => x.Jti == jti)
//            .FirstOrDefaultAsync();
//    }

//    public async Task<UserAuthenticationToken?> GetAuthenticationTokenWithRefreshToken(string refreshToken)
//    {
//        var userAuthenticationToken = await _nextSharpMySQLWriteDbContext.UserAuthenticationTokens
//            .Where(x => x.Value == refreshToken)
//            .Include(x => x.User)
//            .Include(x => x.User!.Customer)
//            .FirstOrDefaultAsync();
        
//        return userAuthenticationToken;
//    }

//    public async Task RevokeAllTokenFromMySQLByUserId(UserId userId)
//    {
//        await _nextSharpMySQLWriteDbContext.UserAuthenticationTokens.Where(x => x.UserId == userId)
//            .ExecuteUpdateAsync(a => a.SetProperty(a => a.IsBlackList, IsBlackList.True));
//    }
//}
