using Domain.Entities.Users;

namespace Domain.IRepositories.Users;

public interface IUserRepository
{
    Task AddUserToPostgreSQL(User user);
    Task<User?> GetUserFromPostgreSQL(UserId userId);
    // Task<bool> IsUserEmail(UserId? userId, Email email);
}
