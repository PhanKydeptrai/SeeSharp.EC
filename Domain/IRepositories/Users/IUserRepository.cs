using Domain.Entities.Users;

namespace Domain.IRepositories.Users;

public interface IUserRepository
{
    Task AddUserToPostgreSQL(User user);
    Task<User?> GetUserFromPostgreSQL(UserId userId);
    void UpdateUser(User user);
    // Task<bool> IsUserEmail(UserId? userId, Email email);
}
