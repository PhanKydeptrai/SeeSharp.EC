using Domain.Primitives;

namespace Domain.Entities.Vouchers;

public sealed class VoucherId : BaseId<VoucherId>
{
    public static readonly VoucherId DefaultVoucherId = FromGuid(new Guid("019758f1-5449-87e0-d68b-e53ea6f1fb6b"));
}
