using Application.DTOs.Employee;
using Application.Features.Pages;
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
    
    /// <summary>
    /// Lấy thông tin nhân viên theo ID
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns></returns>
    Task<Employee?> GetEmployeeById(UserId userId);

    /// <summary>
    /// Lấy thông tin hồ sơ nhân viên theo UserId
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<EmployeeProfileResponse?> GetEmployeeProfileById(UserId userId);

    /// <summary>
    /// Lấy danh sách tất cả nhân viên
    /// </summary>
    /// <param name="statusFilter">Lọc theo trạng thái</param>
    /// <param name="roleFilter">Lọc theo vai trò</param>
    /// <param name="searchTerm">Tìm kiếm theo tên, email hoặc số điện thoại</param>
    /// <param name="sortColumn">Cột sắp xếp</param>
    /// <param name="sortOrder">Thứ tự sắp xếp</param>
    /// <param name="page">Trang</param>
    /// <param name="pageSize">Số lượng mỗi trang</param>
    /// <returns>Danh sách nhân viên phân trang</returns>
    Task<PagedList<EmployeeResponse>> GetAllEmployees(
        string? statusFilter,
        string? roleFilter,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize);
}
