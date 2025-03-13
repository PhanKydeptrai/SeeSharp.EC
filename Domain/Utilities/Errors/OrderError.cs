using Domain.Entities.OrderDetails;
using SharedKernel;

namespace Domain.Utilities.Errors;

public static class OrderError
{
    public static Error NotFoundOrderDetail(OrderDetailId orderDetailId) =>  Error.NotFound(
        "OrderDetatail.NotFound",
        $"The Order detail with the Id = '{orderDetailId}' was not found");
}
