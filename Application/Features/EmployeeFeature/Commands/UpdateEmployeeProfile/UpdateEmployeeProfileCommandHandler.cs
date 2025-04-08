using Application.Abstractions.Messaging;
using Domain.Entities.Employees;
using Domain.Entities.Users;
using Domain.IRepositories;
using Domain.IRepositories.Employees;
using Domain.IRepositories.Users;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.EmployeeFeature.Commands.UpdateEmployeeProfile;

internal sealed class UpdateEmployeeProfileCommandHandler : ICommandHandler<UpdateEmployeeProfileCommand>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEmployeeProfileCommandHandler(
        IEmployeeRepository employeeRepository,
        IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateEmployeeProfileCommand request, CancellationToken cancellationToken)
    {
        // Lấy thông tin employee bao gồm user từ repository
        var employee = await _employeeRepository.GetEmployeeByUserId(UserId.FromGuid(request.UserId));
        if (employee is null)
        {
            return Result.Failure(EmployeeError.NotFound(request.UserId));
        }

        if (employee.User is null)
        {
            return Result.Failure(Error.NotFound("User.NotFound", $"User with ID '{request.UserId}' was not found"));
        }

        // TODO: Xử lý imageFile sẽ được triển khai sau

        // Cập nhật thông tin người dùng sử dụng phương thức UpdateUser
        employee.User.UpdateUser(
            UserName.NewUserName(request.UserName),
            employee.User.Email!,
            PhoneNumber.NewPhoneNumber(request.PhoneNumber),
            request.DateOfBirth,
            employee.User.ImageUrl); // Giữ nguyên ImageUrl hiện tại

        // Lưu thay đổi
        await _unitOfWork.SaveChangeAsync();

        return Result.Success();

    }
}