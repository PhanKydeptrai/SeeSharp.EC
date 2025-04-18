using Application.Abstractions.Messaging;
using Application.DTOs.Bills;

namespace Application.Features.OrderFeature.Queries.GetOrderHistoryByOrderId;

public record GetOrderHistoryByOrderIdQuery(Guid OrderId) : IQuery<BillResponse>;
