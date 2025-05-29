using Application.Abstractions.Messaging;
using Application.IServices;
using Application.Services;
using Domain.Entities.Customers;
using Domain.Entities.Feedbacks;
using Domain.Entities.Orders;
using Domain.IRepositories;
using Domain.IRepositories.Feedbacks;
using Domain.Utilities.Errors;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SharedKernel;

namespace Application.Features.FeedbackFeature.Commands.CreateNewFeedBack;

internal sealed class CreateNewFeedBackCommandHandler : ICommandHandler<CreateNewFeedBackCommand>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IOrderQueryServices _orderQueryServices;
    private readonly CloudinaryService _cloudinaryService;
    private readonly IUnitOfWork _unitOfWork;
    public CreateNewFeedBackCommandHandler(
        IFeedbackRepository feedbackRepository,
        IUnitOfWork unitOfWork,
        IOrderQueryServices orderQueryServices,
        CloudinaryService cloudinaryService)
    {
        _feedbackRepository = feedbackRepository;
        _unitOfWork = unitOfWork;
        _orderQueryServices = orderQueryServices;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<Result> Handle(CreateNewFeedBackCommand request, CancellationToken cancellationToken)
    {
        var orderId = OrderId.FromGuid(request.OrderId);
        var isValidOrder = await _orderQueryServices.IsOrderStatusDelivered(orderId);

        if (!isValidOrder)
        {
            return Result.Failure(OrderError.OrderStatusInValid(orderId));
        }

        // Xử lý ảnh
        //--------------------
        string imageUrl = string.Empty;
        if (request.Image != null)
        {

            //tạo memory stream từ file ảnh
            var memoryStream = new MemoryStream();
            await request.Image.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            //Upload ảnh lên cloudinary

            var resultUpload = await _cloudinaryService.UploadAsync(memoryStream, request.Image.FileName);
            imageUrl = resultUpload.SecureUrl.ToString(); //Nhận url ảnh từ cloudinary
            //Log
            Console.WriteLine(resultUpload.JsonObj);
        }
        //--------------------

        var feedback = Feedback.NewFeedback(
            Substance.FromString(request.Substance),
            RatingScore.FromFloat(request.RatingScore),
            imageUrl,
            orderId,
            CustomerId.FromGuid(request.CustomerId));

        await _feedbackRepository.CreateNewFeedback(feedback);

        return Result.Success(feedback);
    }
}