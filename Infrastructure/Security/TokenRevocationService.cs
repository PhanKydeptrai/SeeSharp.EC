using Application.Security;
using Domain.Entities.UserAuthenticationTokens;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.MySQL;

namespace Infrastructure.Security;

public class TokenRevocationService : ITokenRevocationService
{
    private readonly NextSharpMySQLWriteDbContext _dbContext;

    public TokenRevocationService(NextSharpMySQLWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsTokenRevoked(string jti)
    {
        return await _dbContext.UserAuthenticationTokens
            .AnyAsync(x => x.Jti == jti && x.IsBlackList == IsBlackList.True);
    }
}
