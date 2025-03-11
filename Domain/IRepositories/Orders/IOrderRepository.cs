using Domain.Entities.Orders;

namespace Domain.IRepositories.Orders;

public interface IOrderRepository
{
    Task AddNewOrderToMySQL(Order order);
    Task AddNewOrderToPostgreSQL(Order order);
}
