using Domain.Entities.UserAuthenticationTokens;
using Domain.IRepositories.UserAuthenticationTokens;
using Persistence.Database.MySQL;
using Persistence.Database.PostgreSQL;

namespace Persistence.Repositories.UserAuthenticationTokenRepositories;

internal sealed class UserAuthenticationTokenRepository : IUserAuthenticationTokenRepository
{
    private readonly NextSharpMySQLWriteDbContext _nextSharpMySQLWriteDbContext;
    private readonly NextSharpPostgreSQLWriteDbContext _nextSharpPostgreSQLWriteDbContext;
    public UserAuthenticationTokenRepository(
        NextSharpMySQLWriteDbContext nextSharpMySQLWriteDbContext, 
        NextSharpPostgreSQLWriteDbContext nextSharpPostgreSQLWriteDbContext)
    {
        _nextSharpMySQLWriteDbContext = nextSharpMySQLWriteDbContext;
        _nextSharpPostgreSQLWriteDbContext = nextSharpPostgreSQLWriteDbContext;
    }

    public async Task AddUserAuthenticationTokenToMySQL(
        UserAuthenticationToken accessToken, 
        UserAuthenticationToken refreshToken)
    {
        await _nextSharpMySQLWriteDbContext.UserAuthenticationTokens.AddRangeAsync(accessToken, refreshToken);
    }

    public async Task AddUserAuthenticationTokenToPostgreSQL(
        UserAuthenticationToken accessToken, 
        UserAuthenticationToken refreshToken)
    {
        await _nextSharpPostgreSQLWriteDbContext.UserAuthenticationTokens.AddRangeAsync(accessToken, refreshToken);
    }
}
