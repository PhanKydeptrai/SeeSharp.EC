using Domain.Entities.ShippingInformations;
using SharedKernel;

namespace Domain.Utilities.Errors;

public static class ShippingInformationError
{
    /// <summary>
    /// Shipping Information không tìm thấy
    /// </summary>
    /// <param name="shippingInformationId"></param>
    /// <returns></returns>
    public static Error NotFound(ShippingInformationId shippingInformationId) => Error.NotFound(
        "ShippingInformation.NotFound",
        $"The shipping information with the Id = '{shippingInformationId}' was not found");

    /// <summary>
    /// Không thể đặt thay đổi trạng thái mặc định của thông tin giao hàng
    /// </summary>
    /// <param name="shippingInformationId"></param>
    /// <returns></returns>
    public static Error CannotUnsetDefaultShippingInformation(ShippingInformationId shippingInformationId) => Error.Problem(
        "ShippingInformation.CannotUnsetDefault",
        $"The shipping information with the Id = '{shippingInformationId}' cannot be unset as default because it is the only default shipping information available.");
    
    /// <summary>
    /// Không thể đặt thông tin giao hàng làm mặc định
    /// vì nó đã được đặt làm mặc định trước đó.
    /// </summary>
    /// <param name="shippingInformationId"></param>
    /// <returns></returns>
    public static Error CannotSetDefaultShippingInformation(ShippingInformationId shippingInformationId) => Error.Problem(
        "ShippingInformation.CannotSetDefault",
        $"The shipping information with the Id = '{shippingInformationId}' cannot be set as default because it is already set as default.");
}
