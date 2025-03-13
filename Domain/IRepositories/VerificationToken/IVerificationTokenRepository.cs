using Domain.Entities.VerificationTokens;

namespace Domain.IRepositories.VerificationTokens;

public interface IVerificationTokenRepository
{
    Task<VerificationToken?> GetVerificationTokenFromMySQL(VerificationTokenId verificationTokenId);
    Task AddVerificationTokenToMySQL(VerificationToken token);
    void RemoveVerificationTokenFromMySQL(VerificationToken token);

    // Task<VerificationToken?> GetVerificationTokenFromPostgreSQL(VerificationTokenId verificationTokenId);
    // Task AddVerificationTokenToPostgreSQL(VerificationToken token);
    // void RemoveVerificationTokenFrommPostgreSQL(VerificationToken token);
}
