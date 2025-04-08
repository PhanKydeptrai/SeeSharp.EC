using MediatR;
using SharedKernel;

namespace Application.Features.EmployeeFeature.Commands.UpdateEmployeeStatus;

public record UpdateEmployeeStatusCommand(
    Guid EmployeeId,
    string NewStatus,
    string Token) : IRequest<Result>;