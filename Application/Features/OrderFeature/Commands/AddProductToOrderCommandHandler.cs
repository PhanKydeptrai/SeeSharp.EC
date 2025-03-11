using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Domain.IRepositories;
using Domain.IRepositories.Orders;
using Domain.IRepositories.Products;
using SharedKernel;

namespace Application.Features.OrderFeature.Commands;

internal sealed class AddProductToOrderCommandHandler : ICommandHandler<AddProductToOrderCommand>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IEventBus _eventBus;
    public AddProductToOrderCommandHandler(
        IUnitOfWork unitOfWork,
        IProductRepository productRepository,
        IEventBus eventBus,
        IOrderRepository orderRepository)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _eventBus = eventBus;
        _orderRepository = orderRepository;
    }

    public async Task<Result> Handle(AddProductToOrderCommand request, CancellationToken cancellationToken)
    {
        
        throw new NotImplementedException();
    }
}
