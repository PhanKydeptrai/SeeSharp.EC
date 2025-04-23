using Domain.Entities.Users;

namespace Application.IServices;

public interface IUserQueryService
{
    Task<bool> IsEmailExists(Email email, CancellationToken cancellationToken = default);
} 