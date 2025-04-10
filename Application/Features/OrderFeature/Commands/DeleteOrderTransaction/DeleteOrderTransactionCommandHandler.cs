using Application.Abstractions.Messaging;
using Domain.Entities.Customers;
using Domain.IRepositories;
using Domain.IRepositories.Orders;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.OrderFeature.Commands.DeleteOrderTransaction;

internal sealed class DeleteOrderTransactionCommandHandler : ICommandHandler<DeleteOrderTransactionCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteOrderTransactionCommandHandler(
        IOrderRepository orderRepository, 
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteOrderTransactionCommand request, CancellationToken cancellationToken)
    {
        var customerId = CustomerId.FromGuid(request.CustomerId);
        var orderTransaction = await _orderRepository.GetOrderTransactionByCustomerId(customerId);
        
        if(orderTransaction is null)
        {
            return Result.Failure(OrderError.OrderNotCreated(customerId));
        }

        _orderRepository.RemoveOrderTransaction(orderTransaction);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
