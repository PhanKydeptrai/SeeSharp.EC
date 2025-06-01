//using Application.Abstractions.Messaging;
//using Application.IServices;
//using Application.Services;
//using Domain.Entities.Bills;
//using Domain.Entities.Customers;
//using Domain.Entities.Feedbacks;
//using Domain.IRepositories;
//using Domain.IRepositories.Feedbacks;
//using Domain.Utilities.Errors;
//using SharedKernel;

//namespace Application.Features.FeedbackFeature.Commands.CreateNewFeedBack;

//internal sealed class CreateNewFeedBackCommandHandler : ICommandHandler<CreateNewFeedBackCommand>
//{
//    private readonly IFeedbackRepository _feedbackRepository;
//    private readonly IOrderQueryServices _orderQueryServices;
//    private readonly CloudinaryService _cloudinaryService;
//    private readonly IUnitOfWork _unitOfWork;
//    public CreateNewFeedBackCommandHandler(
//        IFeedbackRepository feedbackRepository,
//        IUnitOfWork unitOfWork,
//        IOrderQueryServices orderQueryServices,
//        CloudinaryService cloudinaryService)
//    {
//        _feedbackRepository = feedbackRepository;
//        _unitOfWork = unitOfWork;
//        _orderQueryServices = orderQueryServices;
//        _cloudinaryService = cloudinaryService;
//    }

//    public async Task<Result> Handle(CreateNewFeedBackCommand request, CancellationToken cancellationToken)
//    {
//        var billId = BillId.FromGuid(request.BillId);
//        //var isValidOrder = await _orderQueryServices.IsOrderStatusDelivered(orderId);

//        if (!isValidOrder)
//        {
//            //return Result.Failure(OrderError.OrderStatusInValid(orderId));
//        }


//        string imageUrl = string.Empty;
//        if (request.Image != null)
//        {
//            imageUrl = await _cloudinaryService.UploadNewImage(request.Image);
//        }

//        var feedback = Feedback.NewFeedback(
//            Substance.FromString(request.Substance),
//            RatingScore.FromFloat(request.RatingScore),
//            imageUrl,
//            billId,
//            CustomerId.FromGuid(request.CustomerId));

//        await _feedbackRepository.CreateNewFeedback(feedback);

//        return Result.Success(feedback);
//    }
//}