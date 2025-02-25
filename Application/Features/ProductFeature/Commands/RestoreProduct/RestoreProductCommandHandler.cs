using Application.Abstractions.Messaging;
using Domain.IRepositories;
using SharedKernel;

namespace Application.Features.ProductFeature.Commands.RestoreProduct;

internal sealed class RestoreProductCommandHandler : ICommandHandler<RestoreProductCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public RestoreProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<Result> Handle(RestoreProductCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
