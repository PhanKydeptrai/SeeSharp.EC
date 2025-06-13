using Application.Abstractions.Messaging;
using Domain.IRepositories;
using Domain.IRepositories.ShippingInformations;
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
        
    }
}
