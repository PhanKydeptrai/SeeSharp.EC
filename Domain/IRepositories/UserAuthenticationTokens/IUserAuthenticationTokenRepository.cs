using Domain.Entities.UserAuthenticationTokens;

namespace Domain.IRepositories.UserAuthenticationTokens;

public interface IUserAuthenticationTokenRepository
{
    Task AddUserAuthenticationTokenToMySQL(UserAuthenticationToken accessToken, UserAuthenticationToken refreshToken);
    Task AddRefreshTokenToMySQL(UserAuthenticationToken refreshToken);
}
