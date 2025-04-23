using Application.Abstractions.Messaging;
using Application.Services;
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
    private readonly CloudinaryService _cloudinaryService;

    public UpdateEmployeeProfileCommandHandler(
        IEmployeeRepository employeeRepository,
        IUnitOfWork unitOfWork,
        CloudinaryService cloudinaryService)
    {
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
        _cloudinaryService = cloudinaryService;
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

        
        if (request.ImageFile is not null)
        {
            string oldimageUrl = employee.User!.ImageUrl!;
            //Xử lý lưu ảnh mới
            string newImageUrl = string.Empty;
            if (request.ImageFile != null)
            {
                //tạo memory stream từ file ảnh
                var memoryStream = new MemoryStream();
                await request.ImageFile.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                //Upload ảnh lên cloudinary
                var resultUpload = await _cloudinaryService.UploadAsync(memoryStream, request.ImageFile.FileName);
                newImageUrl = resultUpload.SecureUrl.ToString(); //Nhận url ảnh từ cloudinary

                //Log                                              
                Console.WriteLine(resultUpload.JsonObj);
            }

            employee.User!.ImageUrl = newImageUrl;


            //Xóa ảnh cũ
            if (oldimageUrl != "")
            {
                //Upload ảnh lên cloudinary
                var resultDelete = await _cloudinaryService.DeleteAsync(oldimageUrl);
                //Log
                Console.WriteLine(resultDelete.JsonObj);
            }
        }

        // Cập nhật thông tin người dùng sử dụng phương thức UpdateUser
        employee.User.UpdateUser(
            UserName.NewUserName(request.UserName),
            PhoneNumber.NewPhoneNumber(request.PhoneNumber),
            request.DateOfBirth,
            employee.User.Gender,
            employee.User.ImageUrl); // Giữ nguyên ImageUrl hiện tại

        // Lưu thay đổi
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
        
    }
}