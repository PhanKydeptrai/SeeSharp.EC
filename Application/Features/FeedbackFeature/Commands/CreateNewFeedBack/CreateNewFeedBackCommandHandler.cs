using Application.Abstractions.Messaging;
using Application.IServices;
using Domain.Entities.Orders;
using Domain.IRepositories;
using Domain.IRepositories.Feedbacks;
using SharedKernel;

namespace Application.Features.FeedbackFeature.Commands.CreateNewFeedBack;

internal sealed class CreateNewFeedBackCommandHandler : ICommandHandler<CreateNewFeedBackCommand>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IOrderQueryServices _orderQueryServices;
    private readonly IUnitOfWork _unitOfWork;
    public CreateNewFeedBackCommandHandler(
        IFeedbackRepository feedbackRepository,
        IUnitOfWork unitOfWork,
        IOrderQueryServices orderQueryServices)
    {
        _feedbackRepository = feedbackRepository;
        _unitOfWork = unitOfWork;
        _orderQueryServices = orderQueryServices;
    }

    public async Task<Result> Handle(CreateNewFeedBackCommand request, CancellationToken cancellationToken)
    {
        var orderId = OrderId.FromGuid(request.OrderId);
        var isValidOrder = await _orderQueryServices.IsOrderStatusDelivered(orderId);

        if (!isValidOrder)
        {
            //return Result.Failure();
        }
    }
}