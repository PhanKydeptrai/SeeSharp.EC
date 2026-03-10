namespace Application.Security;

public interface ITokenRevocationService
{
    /// <summary>
    /// Kiểm tra xem token có bị thu hồi hay không dựa trên jti (JWT ID)
    /// </summary>
    /// <param name="jti"></param>
    /// <returns></returns>
    Task<bool> IsTokenRevoked(string jti);

    /// <summary>
    /// Thu hồi tất cả token trong chuỗi liên quan đến jti (JWT ID) của token đã bị thu hồi
    /// </summary>
    /// <param name="jti"></param>
    /// <returns></returns>
    Task RevokeTokenChainByJtiAsync(string jti);   
}
