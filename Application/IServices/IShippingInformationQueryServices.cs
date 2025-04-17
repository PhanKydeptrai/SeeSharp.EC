using Application.DTOs.ShippingInformation;
using Domain.Entities.Customers;

namespace Application.IServices;

public interface IShippingInformationQueryServices
{
    Task<ShippingInformationResponse?> GetDefaultShippingInformation(CustomerId customerId);
}
