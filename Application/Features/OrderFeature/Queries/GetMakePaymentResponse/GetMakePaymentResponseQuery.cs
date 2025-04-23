using Application.Abstractions.Messaging;
using Application.DTOs.Order;

namespace Application.Features.OrderFeature.Queries.GetMakePaymentResponse;

public record GetMakePaymentResponseQuery(Guid CustomerId) : IQuery<MakePaymentResponse>;