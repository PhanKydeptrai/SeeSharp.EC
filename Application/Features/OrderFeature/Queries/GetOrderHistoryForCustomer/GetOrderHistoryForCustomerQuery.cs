using Application.Abstractions.Messaging;
using Application.DTOs.Order;

namespace Application.Features.OrderFeature.Queries.GetOrderHistoryForCustomer;

public record GetOrderHistoryForCustomerQuery(Guid CustomerId) : IQuery<List<OrderHistoryResponse>>;
