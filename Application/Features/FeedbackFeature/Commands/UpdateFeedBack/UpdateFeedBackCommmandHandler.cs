using Application.Abstractions.Messaging;
using Application.Services;
using Domain.Entities.Feedbacks;
using Domain.IRepositories;
using Domain.IRepositories.Feedbacks;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.FeedbackFeature.Commands.UpdateFeedBack;

internal sealed class UpdateFeedBackCommmandHandler
    : ICommandHandler<UpdateFeedBackCommmand>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CloudinaryService _cloudinaryService;
    public UpdateFeedBackCommmandHandler(
        IFeedbackRepository feedbackRepository,
        IUnitOfWork unitOfWork,
        CloudinaryService cloudinaryService)
    {
        _feedbackRepository = feedbackRepository;
        _unitOfWork = unitOfWork;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<Result> Handle(UpdateFeedBackCommmand request, CancellationToken cancellationToken)
    {
        var feedackId = FeedbackId.FromGuid(request.FeedbackId);
        var feedback = await _feedbackRepository.GetFeedBackById(feedackId);

        if (feedback is null)
        {
            return Result.Failure(FeedBackError.NotFound(feedackId));
        }

        //Xử lý lưu ảnh mới
        string newImageUrl = string.Empty;
        if (request.Image != null)
        {
            //Upload ảnh lên cloudinary
            newImageUrl = await _cloudinaryService.UploadNewImage(request.Image);
        }

        feedback.UpdatFeedback(
            Substance.FromString(request.Substance),
            RatingScore.FromFloat(request.RatingScore),
            newImageUrl);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
