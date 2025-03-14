using Application.Abstractions.Messaging;
using Application.DTOs.Order;

namespace Application.Features.OrderFeature.Queries.GetOrderByOrderId;

public record GetOrderByOrderIdQuery(Guid OrderId) : IQuery<OrderResponse>;
