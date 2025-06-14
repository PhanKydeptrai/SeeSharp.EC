using Application.Abstractions.Messaging;
using Domain.Entities.Customers;
using Domain.Entities.ShippingInformations;
using Domain.IRepositories;
using Domain.IRepositories.ShippingInformations;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.ShippingInformationFeature.Commands.SetDefaultForShippingInformation;

internal sealed class SetDefaultForShippingInformationCommandHandler : ICommandHandler<SetDefaultForShippingInformationCommand>
{
    private readonly IShippingInformationRepository _shippingInformationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetDefaultForShippingInformationCommandHandler(
        IShippingInformationRepository shippingInformationRepository,
        IUnitOfWork unitOfWork)
    {
        _shippingInformationRepository = shippingInformationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(SetDefaultForShippingInformationCommand request, CancellationToken cancellationToken)
    {
        var shippingInformationId = ShippingInformationId.FromGuid(request.ShippingInformationId);
        var customerId = CustomerId.FromGuid(request.CustomerId);
        var shippingInformation = await _shippingInformationRepository.GetCustomerShippingInformationById(shippingInformationId, customerId);
        if (shippingInformation is null)
        {
            return Result.Failure(ShippingInformationError.NotFound(shippingInformationId));
        }

        if (shippingInformation.IsDefault == IsDefault.True)
        {
            return Result.Failure(ShippingInformationError.CannotSetDefaultShippingInformation(shippingInformationId));
        }

        shippingInformation.SetDefault();
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}   