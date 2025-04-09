using Application.Abstractions.Messaging;
using Application.DTOs.Order;

namespace Application.Features.OrderFeature.Queries.GetCartInformationForGuest;

public record GetCartInformationForGuestQuery(Guid GuestId) : IQuery<OrderResponse>;
