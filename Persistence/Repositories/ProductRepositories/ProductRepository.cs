using Domain.Entities.Categories;
using Domain.Entities.Products;
using Domain.IRepositories.Products;
using Microsoft.EntityFrameworkCore;
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

    public async Task AddProductToPostgreSQL(Product product)
    {
        await _postgreSQLWriteDbContext.Products.AddAsync(product);
    }

    public async Task DeleteProductByCategoryFromMySQL(CategoryId id)
    {
        await _mySQLDbContext.Products
            .Where(a => a.CategoryId == id && a.ProductStatus != ProductStatus.Discontinued)
            .ExecuteUpdateAsync(a => a.SetProperty(a => a.ProductStatus, ProductStatus.Discontinued));
    }

    public async Task DeleteProductByCategoryFromPosgreSQL(CategoryId id)
    {
        await _postgreSQLWriteDbContext.Products
            .Where(a => a.CategoryId == id && a.ProductStatus != ProductStatus.Discontinued)
            .ExecuteUpdateAsync(a => a.SetProperty(a => a.ProductStatus, ProductStatus.Discontinued));
    }

    public async Task<Product?> GetProductFromMySQL(ProductId id)
    {
        return await _mySQLDbContext.Products.FindAsync(id);
    }

    public async Task<Product?> GetProductFromPostgreSQL(ProductId id)
    {
        return await _postgreSQLWriteDbContext.Products.FindAsync(id);
    }

    public async Task RestoreProductByCategoryFromMySQL(CategoryId id)
    {
        await _mySQLDbContext.Products
            .Where(a => a.CategoryId == id && a.ProductStatus == ProductStatus.Discontinued)
            .ExecuteUpdateAsync(a => a.SetProperty(a => a.ProductStatus, ProductStatus.InStock));
    }

    public async Task RestoreProductByCategoryFromPostgreSQL(CategoryId id)
    {
        await _postgreSQLWriteDbContext.Products
            .Where(a => a.CategoryId == id && a.ProductStatus != ProductStatus.InStock)
            .ExecuteUpdateAsync(a => a.SetProperty(a => a.ProductStatus, ProductStatus.InStock));
    }
}
