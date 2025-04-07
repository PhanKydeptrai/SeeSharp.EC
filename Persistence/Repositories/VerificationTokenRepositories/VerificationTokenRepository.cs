using Domain.Entities.VerificationTokens;
using Domain.IRepositories.VerificationTokens;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL;

namespace Persistence.Repositories.VerificationTokenRepositories;

internal sealed class VerificationTokenRepository : IVerificationTokenRepository
{
    private readonly SeeSharpPostgreSQLWriteDbContext _nextSharpPostgreSQLWriteDbContext;
    public VerificationTokenRepository(SeeSharpPostgreSQLWriteDbContext nextSharpPostgreSQLWriteDbContext)
    {
        _nextSharpPostgreSQLWriteDbContext = nextSharpPostgreSQLWriteDbContext;
    }

    public async Task AddVerificationTokenToPostgreSQL(VerificationToken token)
    {
        await _nextSharpPostgreSQLWriteDbContext.VerificationTokens.AddAsync(token);
    }

    public async Task<VerificationToken?> GetVerificationTokenFromPostgreSQL(VerificationTokenId verificationTokenId)
    {
        return await _nextSharpPostgreSQLWriteDbContext.VerificationTokens
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.VerificationTokenId == verificationTokenId);
    }

    public void RemoveVerificationTokenFrommPostgreSQL(VerificationToken token)
    {
        _nextSharpPostgreSQLWriteDbContext.VerificationTokens.Remove(token);
    }

    // public async Task<VerificationToken?> GetVerificationTokenFromPostgreSQL(VerificationTokenId verificationTokenId)
    // {
    //     return await _nextSharpPostgreSQLWriteDbContext.VerificationTokens
    //             .Include(a => a.User)
    //             .FirstOrDefaultAsync(a => a.VerificationTokenId == verificationTokenId);
    // }


}
