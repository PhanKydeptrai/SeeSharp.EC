using Domain.Entities.UserAuthenticationTokens;
using Domain.Entities.Users;

namespace Domain.IRepositories.UserAuthenticationTokens;

public interface IUserAuthenticationTokenRepository
{
    // Task AddUserAuthenticationTokenToMySQL(UserAuthenticationToken accessToken, UserAuthenticationToken refreshToken);
    Task AddRefreshTokenToMySQL(UserAuthenticationToken refreshToken);
    Task<UserAuthenticationToken?> GetAuthenticationTokenWithRefreshToken(string refreshToken);
    Task<UserAuthenticationToken?> GetRefreshTokenFromMySQLByJti(string jti);
    // Task<string?> IsRefreshTokenStillCanBeUsed(UserId userId);
}
