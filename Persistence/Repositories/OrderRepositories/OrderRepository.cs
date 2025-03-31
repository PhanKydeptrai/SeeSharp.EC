// using Domain.Entities.Customers;
// using Domain.Entities.OrderDetails;
// using Domain.Entities.Orders;
// using Domain.Entities.OrderTransactions;
// using Domain.Entities.Products;
// using Domain.IRepositories.Orders;
// using Microsoft.EntityFrameworkCore;
// using Persistence.Database.PostgreSQL;

// namespace Persistence.Repositories.OrderRepositories;

// internal sealed class OrderRepository : IOrderRepository
// {
//     private readonly SeeSharpPostgreSQLWriteDbContext _postgreSQLWriteDbContext;

//     public OrderRepository(
//         SeeSharpPostgreSQLWriteDbContext postgreSQLWriteDbContext)
//     {
//         _postgreSQLWriteDbContext = postgreSQLWriteDbContext;
//     }
//     public async Task AddNewOrderToPostgreSQL(Order order)
//     {
//         await _postgreSQLWriteDbContext.Orders.AddAsync(order);
//     }

//     public async Task AddNewOrderDetailToMySQL(OrderDetail orderDetail)
//     {
//         await _postgreSQLWriteDbContext.OrderDetails.AddAsync(orderDetail);
//     }

//     public async Task AddNewOrderDetailToPostgreSQL(OrderDetail orderDetail)
//     {
//         await _postgreSQLWriteDbContext.OrderDetails.AddAsync(orderDetail);
//     }

//     public async Task AddNewOrderTransactionToPostgreSQL(OrderTransaction orderTransaction)
//     {
//         await _postgreSQLWriteDbContext.OrderTransactions.AddAsync(orderTransaction);
//     }

//     public async Task<OrderDetail?> CheckProductAvailabilityInOrder(OrderId orderId, ProductId productId)
//     {
//         return await _postgreSQLWriteDbContext.OrderDetails
//             .FirstOrDefaultAsync(x => x.OrderId == orderId && x.ProductId == productId);
//     }

//     /// <summary>
//     /// Get order detail by OrderDetailId
//     /// </summary>
//     /// <param name="orderDetailId"></param>
//     /// <returns></returns>
//     public async Task<OrderDetail?> GetOrderDetailByIdFromPostgreSQL(OrderDetailId orderDetailId)
//     {
//         return await _postgreSQLWriteDbContext.OrderDetails
//             .Include(a => a.Order)
//             .Where(a => a.OrderDetailId == orderDetailId)
//             .FirstOrDefaultAsync();
//     }

//     /// <summary>
//     /// Get order by OrderId
//     /// </summary>
//     /// <param name="orderId"></param>
//     /// <returns></returns>
//     public async Task<Order?> GetOrderByIdFromPostgreSQL(OrderId orderId)
//     {
//         return await _postgreSQLWriteDbContext.Orders.FindAsync(orderId);
//     }
//     /// <summary>
//     /// Get order by customer id
//     /// </summary>
//     /// <param name="customerId"></param>
//     /// <returns></returns>
//     public async Task<Order?> GetOrderByCustomerIdFromPostgreSQL(CustomerId customerId)
//     {
//         return await _postgreSQLWriteDbContext.Orders
//             .FirstOrDefaultAsync(x => x.CustomerId == customerId);
//     }

//     /// <summary>
//     /// Delete order detail from PostgreSQL
//     /// </summary>
//     /// <param name="orderDetail"></param>
//     public void DeleteOrderDetailFromPostgeSQL(OrderDetail orderDetail)
//     {
//         _postgreSQLWriteDbContext.OrderDetails.Remove(orderDetail);
//     }

// }
