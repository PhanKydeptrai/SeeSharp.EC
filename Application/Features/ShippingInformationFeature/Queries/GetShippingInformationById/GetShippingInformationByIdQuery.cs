using Application.Abstractions.Messaging;
using Application.DTOs.ShippingInformation;

namespace Application.Features.ShippingInformationFeature.Queries.GetShippingInformationById;

public record GetShippingInformationByIdQuery(Guid ShippingInformationId) : IQuery<ShippingInformationResponse>;