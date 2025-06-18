using Application.DTOs.Feedbacks;
using Application.IServices;
using Domain.Entities.Feedbacks;
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
}
