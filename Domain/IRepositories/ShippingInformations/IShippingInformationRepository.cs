using Domain.Entities.Customers;
using Domain.Entities.ShippingInformations;

namespace Domain.IRepositories.ShippingInformations;

public interface IShippingInformationRepository
{
    /// <summary>
    /// Kiểm tra xem có thông tin giao hàng mặc định cho khách hàng hay không.
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>
    Task<bool> IsDefaultShippingInformationExist(CustomerId customerId);

    /// <summary>
    /// Thêm thông tin giao hàng mới vào hệ thống.
    /// </summary>
    /// <param name="shippingInformation"></param>
    /// <returns></returns>
    Task AddNewShippingInformation(ShippingInformation shippingInformation);

    /// <summary>
    /// Lấy thông tin giao hàng theo ID.
    /// </summary>
    /// <param name="shippingInformationId"></param>
    /// <returns></returns>
    Task<ShippingInformation?> GetShippingInformationById(ShippingInformationId shippingInformationId);

    /// <summary>
    /// Kiểm tra xem thông tin giao hàng đã tồn tại hay chưa.
    /// </summary>
    /// <param name="shippingInformationId"></param>
    /// <returns></returns>
    Task<bool> IsExistedShippingInformation(ShippingInformationId shippingInformationId);

    /// <summary>
    /// Lấy thông tin giao hàng mặc định của khách hàng.
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>
    Task<ShippingInformation?> GetDefaultShippingInformation(CustomerId customerId);

    /// <summary>
    /// Xoá thông tin giao hàng.
    /// </summary>
    /// <param name="shippingInformation"></param>
    void RemoveShippingInformation(ShippingInformation shippingInformation); 
}
