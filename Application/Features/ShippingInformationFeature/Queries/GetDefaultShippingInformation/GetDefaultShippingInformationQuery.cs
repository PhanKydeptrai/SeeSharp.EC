using Application.Abstractions.Messaging;
using Application.DTOs.ShippingInformation;

namespace Application.Features.ShippingInformationFeature.Queries.GetDefaultShippingInformation;

public record GetDefaultShippingInformationQuery(Guid CustomerId) : IQuery<ShippingInformationResponse>;
