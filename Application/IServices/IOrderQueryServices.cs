using Domain.Entities.Customers;
using Domain.Entities.Orders;

namespace Application.IServices;

public interface IOrderQueryServices
{
    Task<OrderId?> CheckOrderAvailability(CustomerId customerId);
    
}
