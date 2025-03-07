namespace Application.Security;

public interface ITokenRevocationService
{
    Task<bool> IsTokenRevoked(string jti);
}
