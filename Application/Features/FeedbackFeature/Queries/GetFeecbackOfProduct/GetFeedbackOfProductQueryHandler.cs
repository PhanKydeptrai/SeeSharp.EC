using Application.Abstractions.Messaging;
using Application.DTOs.Feedbacks;
using Application.Features.Pages;
using Application.IServices;
using Domain.Entities.Products;
using Domain.Utilities.Errors;
using Microsoft.AspNetCore.Http;
using SharedKernel;

namespace Application.Features.FeedbackFeature.Queries.GetFeecbackOfProduct;

internal sealed class GetFeedbackOfProductQueryHandler : IQueryHandler<GetFeedbackOfProductQuery, PagedList<FeedbackResponse>>
{
    private readonly IFeedbackQueryServices _feedbackQueryServices;
    private readonly IProductQueryServices _productQueryServices;
    public GetFeedbackOfProductQueryHandler(
        IFeedbackQueryServices feedbackQueryServices,
        IProductQueryServices productQueryServices)
    {
        _feedbackQueryServices = feedbackQueryServices;
        _productQueryServices = productQueryServices;
    }

    public async Task<Result<PagedList<FeedbackResponse>>> Handle(
        GetFeedbackOfProductQuery request,
        CancellationToken cancellationToken)
    {
        var productId = ProductId.FromGuid(request.ProductId);
        var isValidProduct = await _productQueryServices.IsProductExist(productId);
        if (isValidProduct is false)
        {
            return Result.Failure<PagedList<FeedbackResponse>>(ProductError.ProductNotFound(productId));   
        }
        
        return await _feedbackQueryServices.GetFeedbackOfProduct(
                productId,
                request.filter,
                request.sortColumn,
                request.sortOrder,
                request.page,
                request.pageSize);
    }
}
