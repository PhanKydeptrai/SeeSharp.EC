using Application.DTOs.Employee;
using Domain.Entities.Employees;
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

    /// <summary>
    /// Xác thực tài khoản nhân viên
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<EmployeeAuthenticationResponse?> AuthenticateEmployee(Email email, PasswordHash password);
    
    /// <summary>
    /// Lấy thông tin nhân viên theo email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<Employee?> GetEmployeeByEmail(Email email);
}
