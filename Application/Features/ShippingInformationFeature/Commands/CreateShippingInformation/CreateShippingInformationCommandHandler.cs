using Application.Abstractions.Messaging;
using Domain.Entities.Customers;
using Domain.Entities.ShippingInformations;
using Domain.Entities.Users;
using Domain.IRepositories.ShippingInformations;
using SharedKernel;

namespace Application.Features.ShippingInformationFeature.Commands.CreateShippingInformation;

internal sealed class CreateShippingInformationCommandHandler 
    : ICommandHandler<CreateShippingInformationCommand>
{
    private readonly IShippingInformationRepository _shippingInformationRepository;
    public CreateShippingInformationCommandHandler(
        IShippingInformationRepository shippingInformationRepository)
    {
        _shippingInformationRepository = shippingInformationRepository;
    }

    public async Task<Result> Handle(
        CreateShippingInformationCommand request, 
        CancellationToken cancellationToken)
    {
        var shippingInformation = ShippingInformation.NewShippingInformation(
            CustomerId.FromGuid(request.CustomerId),
            FullName.FromString(request.FullName),
            PhoneNumber.FromString(request.PhoneNumber),
            IsDefault.False,
            SpecificAddress.FromString(request.SpecificAddress),
            Province.FromString(request.Province),
            District.FromString(request.District),
            Ward.FromString(request.Ward));

        await _shippingInformationRepository.AddNewShippingInformation(shippingInformation);

        return Result.Success();
    }
}
