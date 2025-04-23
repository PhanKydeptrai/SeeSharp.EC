using Domain.Entities.ShippingInformations;

namespace Domain.IRepositories.ShippingInformations;

public interface IShippingInformationRepository
{
    Task AddNewShippingInformation(ShippingInformation shippingInformation);
    Task<bool> IsExistedShippingInformation(ShippingInformationId shippingInformationId);
}
