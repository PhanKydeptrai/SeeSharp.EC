using Application.DTOs.ShippingInformation;
using Application.IServices;
using Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL;

namespace Infrastructure.Services.ShippingInformationServices;

internal sealed class ShippingInformationQueryServices : IShippingInformationQueryServices
{
    private readonly SeeSharpPostgreSQLReadDbContext _dbContext;
    public ShippingInformationQueryServices(SeeSharpPostgreSQLReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ShippingInformationResponse?> GetDefaultShippingInformation(CustomerId customerId)
    {
        var result = await _dbContext.ShippingInformations
            .Where(x => x.CustomerId == new Ulid(customerId) && x.IsDefault)
            .Select(x => new ShippingInformationResponse(
                x.ShippingInformationId.ToGuid(),
                x.CustomerId.ToGuid(),
                x.FullName,
                x.PhoneNumber,
                x.IsDefault,
                x.SpecificAddress,
                x.Province,
                x.District,
                x.Ward))
            .FirstOrDefaultAsync();

        return result;
    }
}
