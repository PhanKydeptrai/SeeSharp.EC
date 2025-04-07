using Domain.Entities.Users;

namespace Application.IServices;

public interface IEmployeeQueryServices
{
    /// <summary>
    /// Kiểm tra email có tồn tại trong bảng nhân viên hay không.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsEmailExists(Email email);

    /// <summary>
    /// Kiểm tra số điện thoại có tồn tại trong bảng nhân viên hay không.
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    Task<bool> IsPhoneNumberExists(PhoneNumber phoneNumber);
}
