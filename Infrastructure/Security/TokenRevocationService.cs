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
        var token = await _dbContext.UserAuthenticationTokens.FirstOrDefaultAsync(x => x.Jti == jti);
        
        if(token is null)
        {
            return true; // Token is invalid
        }

        if(token is not null && token.IsBlackList == IsBlackList.True)
        {
            return true; // Token is revoked
        }
        
        return false;
    }
}
