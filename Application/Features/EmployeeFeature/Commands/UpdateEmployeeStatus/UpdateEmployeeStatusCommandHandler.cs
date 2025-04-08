using Domain.Entities.Users;
using Domain.IRepositories;
using Domain.IRepositories.Employees;
using MediatR;
using SharedKernel;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Features.EmployeeFeature.Commands.UpdateEmployeeStatus;

public class UpdateEmployeeStatusCommandHandler : IRequestHandler<UpdateEmployeeStatusCommand, Result>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEmployeeStatusCommandHandler(
        IEmployeeRepository employeeRepository,
        IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateEmployeeStatusCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetEmployeeByUserId(UserId.FromGuid(request.EmployeeId));
        
        if (employee is null)
        {
            return Result.Failure(Error.NotFound("Employee", "Employee not found"));
        }

        // Lấy sub từ token
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(request.Token);
        var sub = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

        // Kiểm tra xem người dùng đang đăng nhập có phải là người đang được cập nhật không
        if (sub == request.EmployeeId.ToString())
        {
            return Result.Failure(Error.NotFound("Status", "Cannot update your own status"));
        }

        if (employee.User != null)
        {
            if (!Enum.TryParse<UserStatus>(request.NewStatus, true, out var status))
            {
                return Result.Failure(Error.NotFound("Status", "Invalid status. Valid values are: Active, InActive, Deleted, Blocked"));
            }

            employee.User.UpdateStatus(status);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        return Result.Success();
    }
}