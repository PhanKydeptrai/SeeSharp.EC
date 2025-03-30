using Domain.Entities.Categories;
using Domain.Entities.Products;
using Domain.IRepositories.Products;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL;

namespace Persistence.Repositories.ProductRepositories;

internal sealed class ProductRepository : IProductRepository
{
    
    private readonly SeeSharpPostgreSQLWriteDbContext _postgreSQLWriteDbContext;

    public ProductRepository(
        SeeSharpPostgreSQLWriteDbContext postgreSQLWriteDbContext)
    {
        _postgreSQLWriteDbContext = postgreSQLWriteDbContext;
    }

    

    public async Task AddProductToPostgreSQL(Product product)
    {
        await _postgreSQLWriteDbContext.Products.AddAsync(product);
    }
    

    public async Task DeleteProductByCategoryFromPosgreSQL(CategoryId id)
    {
        await _postgreSQLWriteDbContext.Products
            .Where(a => a.CategoryId == id)
            .ExecuteUpdateAsync(a => a.SetProperty(a => a.CategoryId, CategoryId.DefaultCategoryId));
    }

    

    public async Task<Product?> GetProductFromPostgreSQL(ProductId id)
    {
        return await _postgreSQLWriteDbContext.Products.FindAsync(id);
    }

    

    public async Task RestoreProductByCategoryFromPostgreSQL(CategoryId id)
    {
        await _postgreSQLWriteDbContext.Products
            .Where(a => a.CategoryId == id && a.ProductStatus != ProductStatus.InStock)
            .ExecuteUpdateAsync(a => a.SetProperty(a => a.ProductStatus, ProductStatus.InStock));
    }
}
