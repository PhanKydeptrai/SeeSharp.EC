using Domain.Entities.Users;

namespace Domain.IRepositories.Users;

public interface IUserRepository
{
    Task AddUserToMySQL(User user);
    Task AddUserToPostgreSQL(User user);
    // Task<bool> IsUserEmail(UserId? userId, Email email);
}
