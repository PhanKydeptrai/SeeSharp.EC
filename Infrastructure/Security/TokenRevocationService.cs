using Application.Security;
using Domain.Entities.UserAuthenticationTokens;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL;

namespace Infrastructure.Security;

public class TokenRevocationService : ITokenRevocationService
{
    private readonly SeeSharpPostgreSQLWriteDbContext _dbContext;

    public TokenRevocationService(SeeSharpPostgreSQLWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsTokenRevoked(string jti)
    {
        return await _dbContext.UserAuthenticationTokens
            .AnyAsync(x => x.Jti == jti && x.IsBlackList == IsBlackList.True);
    }
}
