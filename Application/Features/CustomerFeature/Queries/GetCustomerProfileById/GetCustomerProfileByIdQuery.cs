using Application.Abstractions.Messaging;
using Application.DTOs.Customer;

namespace Application.Features.CustomerFeature.Queries.GetCustomerProfileById;

public record GetCustomerProfileByIdQuery(Guid UserId) : IQuery<CustomerProfileResponse>;
