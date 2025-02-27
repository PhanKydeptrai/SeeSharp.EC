using Domain.Entities.Users;
using Domain.IRepositories.Users;
using Persistence.Database.MySQL;
using Persistence.Database.PostgreSQL;

namespace Persistence.Repositories.UserRepositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly NextSharpMySQLWriteDbContext _mySqlDbContext;
    private readonly NextSharpPostgreSQLWriteDbContext _postgreSQLDbContext;

    public UserRepository(
        NextSharpMySQLWriteDbContext mySqlDbContext, 
        NextSharpPostgreSQLWriteDbContext postgreSQLDbContext)
    {
        _mySqlDbContext = mySqlDbContext;
        _postgreSQLDbContext = postgreSQLDbContext;
    }

    public async Task AddUserToMySQL(User user)
    {
        await _mySqlDbContext.Users.AddAsync(user);
    }

    public async Task AddUserToPostgreSQL(User user)
    {
        await _postgreSQLDbContext.Users.AddAsync(user);
    }
}
