using Application.Abstractions.Messaging;
using Domain.Entities.ShippingInformations;
using Domain.IRepositories;
using Domain.IRepositories.ShippingInformations;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.ShippingInformationFeature.Commands.DeleteShippingInformation;

internal sealed class DeleteShippingInformationCommandHandler : ICommandHandler<DeleteShippingInformationCommand>
{
    private readonly IShippingInformationRepository _shippingInformationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteShippingInformationCommandHandler(
        IShippingInformationRepository shippingInformationRepository,
        IUnitOfWork unitOfWork)
    {
        _shippingInformationRepository = shippingInformationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteShippingInformationCommand request, CancellationToken cancellationToken)
    {
        var shippingInformationId = ShippingInformationId.FromGuid(request.ShippingInformationId);
        var shippingInformation = await _shippingInformationRepository.GetShippingInformationById(shippingInformationId);
        if (shippingInformation is null)
        {
            return Result.Failure(ShippingInformationError.NotFound(shippingInformationId));
        }

        _shippingInformationRepository.DeleteShippingInformation(shippingInformation);

        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}
