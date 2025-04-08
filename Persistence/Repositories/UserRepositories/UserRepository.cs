using Domain.Entities.Users;
using Domain.IRepositories.Users;
using Persistence.Database.PostgreSQL;

namespace Persistence.Repositories.UserRepositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly SeeSharpPostgreSQLWriteDbContext _postgreSQLDbContext;

    public UserRepository(
        SeeSharpPostgreSQLWriteDbContext postgreSQLDbContext)
    {
        _postgreSQLDbContext = postgreSQLDbContext;
    }
    public async Task AddUserToPostgreSQL(User user)
    {
        await _postgreSQLDbContext.Users.AddAsync(user);
    }

    public async Task<User?> GetUserFromPostgreSQL(UserId userId)
    {
        return await _postgreSQLDbContext.Users.FindAsync(userId);
    }
    
    public void UpdateUser(User user)
    {
        _postgreSQLDbContext.Users.Update(user);
    }
}
