using Application.Abstractions.Messaging;
using Application.DTOs.Customer;

namespace Application.Features.CustomerFeature.Queries.GetCustomerProfile;

public record GetCustomerProfileQuery(Guid UserId) : IQuery<CustomerProfileResponse>;
