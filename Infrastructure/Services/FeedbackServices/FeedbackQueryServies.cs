using System.Linq.Expressions;
using Application.DTOs.Feedbacks;
using Application.Features.Pages;
using Application.IServices;
using Domain.Database.PostgreSQL.ReadModels;
using Domain.Entities.Feedbacks;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL;

namespace Infrastructure.Services.FeedbackServices;

public class FeedbackQueryServices : IFeedbackQueryServices
{
    private readonly SeeSharpPostgreSQLReadDbContext _dbContext;

    public FeedbackQueryServices(SeeSharpPostgreSQLReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<FeedbackResponse?> GetFeedbackById(FeedbackId feedbackId)
    {
        return await _dbContext.Feedbacks
            .Where(f => f.FeedbackId == feedbackId.ToUlid())
            .Select(f => new FeedbackResponse(
                f.FeedbackId.ToGuid(),
                f.Substance,
                f.RatingScore,
                f.ImageUrl,
                f.BillId.ToGuid(), f.CustomerId.ToGuid()))
                .FirstOrDefaultAsync();
    }

    public async Task<PagedList<FeedbackResponse>> GetFeedbackOfProduct(
        ProductId productId,
        string? filter,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize)
    {
        var query = _dbContext.Feedbacks
            // .Include(a => a.BillReadModel)
            // .ThenInclude(a => a.Order)
            // .ThenInclude(a => a.OrderDetailReadModels)
            // .ThenInclude(a => a.ProductVariantReadModel.ProductId)
            .Where(f => f.BillReadModel.Order.OrderDetailReadModels
                .Any(od => od.ProductVariantReadModel.ProductId == productId.ToUlid()))
            .AsQueryable();

        //Filter
        if (!string.IsNullOrEmpty(filter)) // lọc theo điểm đánh giá
        {
            if (filter == "1")
            {
                query = query.Where(f => f.RatingScore == 1);
            }
            else if (filter == "2")
            {
                query = query.Where(f => f.RatingScore == 2);
            }
            else if (filter == "3")
            {
                query = query.Where(f => f.RatingScore == 3);
            }
            else if (filter == "4")
            {
                query = query.Where(f => f.RatingScore == 4);
            }
            else if (filter == "5")
            {
                query = query.Where(f => f.RatingScore == 5);
            }
        }

        //sort
        Expression<Func<FeedbackReadModel, object>> keySelector = sortColumn?.ToLower() switch
        {
            "feedbackid" => x => x.FeedbackId,
            "ratingscore" => x => x.RatingScore,
            _ => x => x.FeedbackId
        };

        if (sortOrder?.ToLower() == "desc")
        {
            query = query.OrderByDescending(keySelector);
        }
        else
        {
            query = query.OrderBy(keySelector);
        }

        //paged
        var feedbacks = query
            .Select(a => new FeedbackResponse(
                a.FeedbackId.ToGuid(),
                a.Substance,
                a.RatingScore,
                a.ImageUrl,
                a.BillId.ToGuid(),
                a.CustomerId.ToGuid())).AsQueryable();

        var feedbacksList = await PagedList<FeedbackResponse>
            .CreateAsync(feedbacks, page ?? 1, pageSize ?? 10);
        
        return feedbacksList;
    }
}
