using Application.Abstractions.Messaging;
using Application.Services;
using Domain.Entities.Users;
using Domain.IRepositories;
using Domain.IRepositories.Customers;
using Domain.Utilities.Errors;
using Microsoft.Extensions.Configuration;
using SharedKernel;

namespace Application.Features.CustomerFeature.Commands.UpdateCustomerProfile;

internal sealed class UpdateCustomerProfileCommandHandler : ICommandHandler<UpdateCustomerProfileCommand>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly CloudinaryService _cloudinaryService;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateCustomerProfileCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        CloudinaryService cloudinaryService,
        IConfiguration configuration)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _cloudinaryService = cloudinaryService;
        _configuration = configuration;
    }

    public async Task<Result> Handle(UpdateCustomerProfileCommand request, CancellationToken cancellationToken)
    {
        var userId = UserId.FromGuid(request.UserId);
        var customer = await _customerRepository.GetCustomerByUserId(userId);

        if (customer is null)
        {
            return Result.Failure(CustomerError.NotFoundCustomer());
        }

        if (request.ImageFile is not null)
        {
            string oldimageUrl = customer.User!.ImageUrl!;
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

            customer.User!.ImageUrl = newImageUrl;


            //Xóa ảnh cũ
            if (oldimageUrl != "")
            {
                //Upload ảnh lên cloudinary
                var resultDelete = await _cloudinaryService.DeleteAsync(oldimageUrl);
                //Log
                Console.WriteLine(resultDelete.JsonObj);
            }
        }

        customer.User!.UpdateUser(
                UserName.FromString(request.UserName),
                PhoneNumber.FromString(request.PhoneNumber),
                request.DateOfBirth,
                (Gender)request.Gender,
                customer.User!.ImageUrl);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
