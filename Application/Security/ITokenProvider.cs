using Domain.Entities.Customers;
using Domain.Entities.Employees;
using Domain.Entities.Users;

namespace Application.Security;

public interface ITokenProvider
{
    /// <summary>
    /// Tạo access token cho khách hàng
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="customerId"></param>
    /// <param name="email"></param>
    /// <param name="role"></param>
    /// <param name="jti"></param>
    /// <returns></returns>
    string GenerateAccessTokenForCustomer(UserId userId, CustomerId customerId, Email email, string role, string jti);
    
    /// <summary>
    /// Tạo access token cho nhân viên
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="employeeId"></param>
    /// <param name="email"></param>
    /// <param name="role"></param>
    /// <param name="jti"></param>
    /// <returns></returns>
    string GenerateAccessTokenForEmployee(UserId userId, EmployeeId employeeId, Email email, string role, string jti);
    
    /// <summary>
    /// Tạo access token cho khách hàng chưa đăng ký
    /// </summary>
    /// <returns></returns>
    Task<string> GenerateAccessTokenForGuest();

    /// <summary>
    /// Tạo refresh token 
    /// </summary>
    /// <returns></returns>
    string GenerateRefreshToken();
    
    /// <summary>
    /// tạo chuỗi ngẫu nhiên với độ dài cho trước
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    string GenerateRandomString(int length);
}