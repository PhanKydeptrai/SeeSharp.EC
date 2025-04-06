using Domain.Primitives;

namespace Domain.Entities.Users;
public sealed class UserId : BaseId<UserId>
{
    public static readonly UserId RootUserId = FromGuid(new Guid("01960aec-bac7-71c5-cfb0-309df6c12572"));
}
