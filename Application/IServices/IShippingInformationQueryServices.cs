using Application.DTOs.ShippingInformation;
using Domain.Entities.Customers;
using Domain.Entities.ShippingInformations;

namespace Application.IServices;

public interface IShippingInformationQueryServices
{
    Task<List<ShippingInformationResponse>> GetAllShippingInformation(CustomerId customerId);
    Task<ShippingInformationResponse?> GetShippingInformationById(ShippingInformationId shippingInformationId);
    Task<bool> IsThisShippingInformationDefault(ShippingInformationId shippingInformationId);
    Task<ShippingInformationResponse?> GetDefaultShippingInformation(CustomerId customerId);
}
