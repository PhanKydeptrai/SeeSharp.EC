using Domain.Primitives;

namespace Domain.Entities.Employees;

public sealed class EmployeeId : BaseId<EmployeeId>
{
    public static readonly EmployeeId RootAccountId = FromGuid(new Guid("01960aed-ac00-5c87-4826-7bf26a5d84ac"));
}

