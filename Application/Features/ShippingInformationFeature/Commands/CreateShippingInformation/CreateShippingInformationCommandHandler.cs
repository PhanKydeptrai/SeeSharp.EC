using Application.Abstractions.Messaging;
using Domain.Entities.Customers;
using Domain.Entities.ShippingInformations;
using Domain.Entities.Users;
using Domain.IRepositories;
using Domain.IRepositories.ShippingInformations;
using SharedKernel;

namespace Application.Features.ShippingInformationFeature.Commands.CreateShippingInformation;

internal sealed class CreateShippingInformationCommandHandler 
    : ICommandHandler<CreateShippingInformationCommand>
{
    private readonly IShippingInformationRepository _shippingInformationRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CreateShippingInformationCommandHandler(
        IShippingInformationRepository shippingInformationRepository,
        IUnitOfWork unitOfWork)
    {
        _shippingInformationRepository = shippingInformationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        CreateShippingInformationCommand request, 
        CancellationToken cancellationToken)
    {
        var customerId = CustomerId.FromGuid(request.CustomerId);

        var shippingInformationCheck = await _shippingInformationRepository.IsDefaultShippingInformationExist(customerId);

        var isDefault = IsDefault.True;

        if (shippingInformationCheck) 
        {
            isDefault = IsDefault.False;
        }

        var shippingInformation = ShippingInformation.NewShippingInformation(
                CustomerId.FromGuid(request.CustomerId),
                FullName.FromString(request.FullName),
                PhoneNumber.FromString(request.PhoneNumber),
                isDefault,
                SpecificAddress.FromString(request.SpecificAddress),
                Province.FromString(request.Province),
                District.FromString(request.District),
                Ward.FromString(request.Ward));

        await _shippingInformationRepository.AddNewShippingInformation(shippingInformation);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}
