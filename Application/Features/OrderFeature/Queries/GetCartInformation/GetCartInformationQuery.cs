using Application.Abstractions.Messaging;
using Application.DTOs.Order;

namespace Application.Features.OrderFeature.Queries.GetCartInformation;

public record GetCartInformationQuery(Guid CustomerId) : IQuery<OrderResponse>;
