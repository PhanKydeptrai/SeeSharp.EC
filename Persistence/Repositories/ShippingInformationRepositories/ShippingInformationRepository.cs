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

    public async Task<bool> IsExistedShippingInformation(ShippingInformationId shippingInformationId)
    {
        return await _postgreSQLWriteDbContext.ShippingInformations.AnyAsync(x => x.ShippingInformationId == shippingInformationId);
    }
}
