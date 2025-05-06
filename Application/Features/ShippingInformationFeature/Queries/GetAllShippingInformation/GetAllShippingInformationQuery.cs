using Application.Abstractions.Messaging;
using Application.DTOs.ShippingInformation;

namespace Application.Features.ShippingInformationFeature.Queries.GetAllShippingInformation;

public record GetAllShippingInformationQuery(Guid customerId) : IQuery<List<ShippingInformationResponse>>;
