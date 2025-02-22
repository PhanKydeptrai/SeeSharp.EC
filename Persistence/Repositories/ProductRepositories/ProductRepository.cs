using Domain.Entities.Products;
using Domain.IRepositories.Categories;
using Persistence.Database.MySQL;
using Persistence.Database.PostgreSQL;

namespace Persistence.Repositories.ProductRepositories;

internal sealed class ProductRepository : IProductRepository
{
    private readonly NextSharpMySQLWriteDbContext _mySQLDbContext;
    private readonly NextSharpPostgreSQLWriteDbContext _postgreSQLWriteDbContext;

    public ProductRepository(
        NextSharpPostgreSQLWriteDbContext postgreSQLWriteDbContext, 
        NextSharpMySQLWriteDbContext mySQLDbContext)
    {
        _postgreSQLWriteDbContext = postgreSQLWriteDbContext;
        _mySQLDbContext = mySQLDbContext;
    }

    public async Task AddProductToMySQL(Product product)
    {
        await _mySQLDbContext.Products.AddAsync(product);
    }

    public async Task AddProductToPosgreSQL(Product product)
    {
        await _postgreSQLWriteDbContext.Products.AddAsync(product);
    }


}
