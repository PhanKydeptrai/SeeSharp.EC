using Domain.Entities.Users;

namespace Domain.IRepositories.Users;

public interface IUserRepository
{
    Task AddUserToMySQL(User user);
    Task AddUserToPostgreSQL(User user);
    Task<User?> GetUserFromMySQL(UserId userId);
    Task<User?> GetUserFromPostgreSQL(UserId userId);
    // Task<bool> IsUserEmail(UserId? userId, Email email);
}
