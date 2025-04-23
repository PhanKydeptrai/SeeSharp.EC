using Application.IServices;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL;

namespace Infrastructure.Services.UserServices;

internal sealed class UserQueryService : IUserQueryService
{
    private readonly SeeSharpPostgreSQLReadDbContext _dbContext;
    
    public UserQueryService(SeeSharpPostgreSQLReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsEmailExists(Email email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.AnyAsync(
            u => u.Email == email.Value, 
            cancellationToken);
    }
} 