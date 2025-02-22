using Domain.Entities.Products;

namespace Domain.IRepositories.Categories;

public interface IProductRepository
{
    Task AddProductToMySQL(Product product);
    Task AddProductToPosgreSQL(Product product);
}
