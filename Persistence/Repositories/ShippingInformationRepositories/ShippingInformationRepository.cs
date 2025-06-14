using Domain.Entities.Customers;
using Domain.Entities.ShippingInformations;
using Domain.IRepositories.ShippingInformations;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL;

namespace Persistence.Repositories.ShippingInformationRepositories;

internal sealed class ShippingInformationRepository : IShippingInformationRepository
{
    private readonly SeeSharpPostgreSQLWriteDbContext _postgreSQLWriteDbContext;
    public ShippingInformationRepository(SeeSharpPostgreSQLWriteDbContext postgreSQLWriteDbContext)
    {
        _postgreSQLWriteDbContext = postgreSQLWriteDbContext;
    }

    public async Task AddNewShippingInformation(ShippingInformation shippingInformation)
    {
        await _postgreSQLWriteDbContext.ShippingInformations.AddAsync(shippingInformation);
    }

    public async Task<ShippingInformation?> GetDefaultShippingInformation(CustomerId customerId)
    {
        return await _postgreSQLWriteDbContext.ShippingInformations.FirstOrDefaultAsync(
            x => x.CustomerId == customerId
            && x.IsDefault == IsDefault.True);
    }

    public async Task<ShippingInformation?> GetShippingInformationById(
        ShippingInformationId shippingInformationId)
    {
        return await _postgreSQLWriteDbContext.ShippingInformations.FindAsync(shippingInformationId);
    }

    public async Task<bool> IsDefaultShippingInformationExist(CustomerId customerId)
    {
        return await _postgreSQLWriteDbContext.ShippingInformations.AnyAsync(
            x => x.CustomerId == customerId
            && x.IsDefault == IsDefault.True);
    }

    public async Task<bool> IsExistedShippingInformation(
        ShippingInformationId shippingInformationId)
    {
        return await _postgreSQLWriteDbContext.ShippingInformations.AnyAsync(
            x => x.ShippingInformationId == shippingInformationId);
    }

    public void DeleteShippingInformation(ShippingInformation shippingInformation)
    {
        _postgreSQLWriteDbContext.ShippingInformations.Remove(shippingInformation);
    }

    public async Task<ShippingInformation?> GetCustomerShippingInformationById(
        ShippingInformationId shippingInformationId,
        CustomerId customerId)
    {
        return await _postgreSQLWriteDbContext.ShippingInformations
            .FirstOrDefaultAsync(x => x.ShippingInformationId == shippingInformationId
                                      && x.CustomerId == customerId);
    }
}
