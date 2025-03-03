using Domain.Entities.VerificationTokens;
using Domain.IRepositories.VerificationTokens;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.MySQL;
using Persistence.Database.PostgreSQL;

namespace Persistence.Repositories.VerificationTokenRepositories;

internal sealed class VerificationTokenRepository : IVerificationTokenRepository
{
    private readonly NextSharpMySQLWriteDbContext _nextSharpMySQLWriteDbContext;
    private readonly NextSharpPostgreSQLWriteDbContext _nextSharpPostgreSQLWriteDbContext;
    public VerificationTokenRepository(
        NextSharpMySQLWriteDbContext nextSharpMySQLWriteDbContext,
        NextSharpPostgreSQLWriteDbContext nextSharpPostgreSQLWriteDbContext)
    {
        _nextSharpMySQLWriteDbContext = nextSharpMySQLWriteDbContext;
        _nextSharpPostgreSQLWriteDbContext = nextSharpPostgreSQLWriteDbContext;
    }

    public async Task AddVerificationTokenToMySQL(VerificationToken token)
    {
        await _nextSharpMySQLWriteDbContext.VerificationTokens.AddAsync(token);
    }

    public async Task AddVerificationTokenToPostgreSQL(VerificationToken token)
    {
        await _nextSharpPostgreSQLWriteDbContext.VerificationTokens.AddAsync(token);
    }

    public async Task<VerificationToken?> GetVerificationTokenFromMySQL(VerificationTokenId verificationTokenId)
    {
        return await _nextSharpMySQLWriteDbContext.VerificationTokens
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.VerificationTokenId == verificationTokenId);
    }

    public async Task<VerificationToken?> GetVerificationTokenFromPostgreSQL(VerificationTokenId verificationTokenId)
    {
        return await _nextSharpPostgreSQLWriteDbContext.VerificationTokens
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.VerificationTokenId == verificationTokenId);
    }

    public void RemoveVerificationTokenFrommMySQL(VerificationToken token)
    {
        _nextSharpMySQLWriteDbContext.VerificationTokens.Remove(token);
    }

    public void RemoveVerificationTokenFrommPostgreSQL(VerificationToken token)
    {
        _nextSharpPostgreSQLWriteDbContext.VerificationTokens.Remove(token);
    }
}
