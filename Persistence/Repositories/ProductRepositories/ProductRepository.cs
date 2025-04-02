using Domain.Entities.Categories;
using Domain.Entities.Products;
using Domain.Entities.ProductVariants;
using Domain.IRepositories.Products;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL;

namespace Persistence.Repositories.ProductRepositories;

internal sealed class ProductRepository : IProductRepository
{
    
    private readonly SeeSharpPostgreSQLWriteDbContext _postgreSQLWriteDbContext;

    public ProductRepository(SeeSharpPostgreSQLWriteDbContext postgreSQLWriteDbContext)
    {
        _postgreSQLWriteDbContext = postgreSQLWriteDbContext;
    }
    public async Task AddProduct(Product product)
    {
        await _postgreSQLWriteDbContext.Products.AddAsync(product);
    }

    public async Task AddProductVariant(ProductVariant productVariant)
    {
        await _postgreSQLWriteDbContext.ProductVariants.AddAsync(productVariant);
    }

    public async Task DeleteProductByCategory(CategoryId id)
    {
        await _postgreSQLWriteDbContext.Products
            .Where(a => a.CategoryId == id)
            .ExecuteUpdateAsync(a => a.SetProperty(a => a.CategoryId, CategoryId.DefaultCategoryId));
    }

    public async Task DeleteProductVariantByCategory(CategoryId id)
    {
        await _postgreSQLWriteDbContext.ProductVariants.Include(a => a.Product)
            .Where(a => a.Product!.CategoryId == id)
            .ExecuteUpdateAsync(a => a.SetProperty(a => a.ProductVariantStatus, ProductVariantStatus.Discontinued));
    }

    public async Task DeleteProductVariantByProduct(ProductId id)
    {
        await _postgreSQLWriteDbContext.ProductVariants
            .Where(a => a.ProductId == id)
            .ExecuteUpdateAsync(a => a.SetProperty(a => a.ProductVariantStatus, ProductVariantStatus.Discontinued));
    }

    public async Task<Product?> GetProduct(ProductId id)
    {
        return await _postgreSQLWriteDbContext.Products.FindAsync(id);
    }
    public async Task RestoreProductByCategory(CategoryId id)
    {
        await _postgreSQLWriteDbContext.Products
            .Where(a => a.CategoryId == id && a.ProductStatus != ProductStatus.InStock)
            .ExecuteUpdateAsync(a => a.SetProperty(a => a.ProductStatus, ProductStatus.InStock));
    }

    public async Task RestoreProductVariantByCategory(CategoryId id)
    {
        await _postgreSQLWriteDbContext.ProductVariants.Include(a => a.Product)
            .Where(a => a.Product!.CategoryId == id && a.ProductVariantStatus != ProductVariantStatus.InStock)
            .ExecuteUpdateAsync(a => a.SetProperty(a => a.ProductVariantStatus, ProductVariantStatus.InStock));
    }

    public async Task RestoreProductVariantByProduct(ProductId id)
    {
        await _postgreSQLWriteDbContext.ProductVariants
            .Where(a => a.ProductId == id && a.ProductVariantStatus != ProductVariantStatus.InStock)
            .ExecuteUpdateAsync(a => a.SetProperty(a => a.ProductVariantStatus, ProductVariantStatus.InStock));
    }
}
