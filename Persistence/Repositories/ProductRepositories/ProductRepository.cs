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

    public async Task<Product?> GetProductFromMySQL(ProductId id)
    {
        return await _mySQLDbContext.Products.FindAsync(id);
    }

    public async Task<Product?> GetProductFromPosgreSQL(ProductId id)
    {
        return await _postgreSQLWriteDbContext.Products.FindAsync(id);
    }
}
