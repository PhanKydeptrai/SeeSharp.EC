using Domain.Entities.Customers;
using Domain.Entities.Feedbacks;
using Domain.IRepositories.Feedbacks;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL;

namespace Persistence.Repositories.FeedbackRepositories;

internal sealed class FeedbackRepository : IFeedbackRepository
{
    private readonly SeeSharpPostgreSQLWriteDbContext _dbcontext;

    public FeedbackRepository(SeeSharpPostgreSQLWriteDbContext dbcontext)
    {
        _dbcontext = dbcontext;
    }

    public async Task CreateNewFeedback(Feedback feedback)
    {
        await _dbcontext.Feedbacks.AddAsync(feedback);
    }

    public async Task<Feedback?> GetFeedBackById(FeedbackId feedbackId)
    {
        return await _dbcontext.Feedbacks.FindAsync(feedbackId);
    }

    public async Task<Feedback?> GetFeedBackOfCustomerById(FeedbackId feedbackId, CustomerId customerId)
    {
        return await _dbcontext.Feedbacks.FirstOrDefaultAsync(f => f.FeedbackId == feedbackId && f.CustomerId == customerId);
    }
}
