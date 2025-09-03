using Application.DTOs.ShippingInformation;
using Domain.Entities.Customers;
using Domain.Entities.ShippingInformations;

namespace Application.IServices;

public interface IShippingInformationQueryServices
{
    /// <summary>
    /// Lấy tất cả thông tin vận chuyển của khách hàng
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>
    Task<List<ShippingInformationResponse>> GetAllShippingInformation(CustomerId customerId);

    /// <summary>
    /// Lấy thông tin vận chuyển theo ID
    /// </summary>
    /// <param name="shippingInformationId"></param>
    /// <returns></returns>
    Task<ShippingInformationResponse?> GetShippingInformationById(ShippingInformationId shippingInformationId);

    /// <summary>
    /// Kiểm tra xem thông tin vận chuyển có phải là thông tin mặc định hay không
    /// </summary>
    /// <param name="shippingInformationId"></param>
    /// <returns></returns>
    Task<bool> IsThisShippingInformationDefault(ShippingInformationId shippingInformationId);

    /// <summary>
    /// Lấy thông tin vận chuyển mặc định của khách hàng
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>
    Task<ShippingInformationResponse?> GetDefaultShippingInformation(CustomerId customerId);
}
