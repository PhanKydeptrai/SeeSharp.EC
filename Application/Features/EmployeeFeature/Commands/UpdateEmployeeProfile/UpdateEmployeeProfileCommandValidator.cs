using FluentValidation;
using System.Text.RegularExpressions;

namespace Application.Features.EmployeeFeature.Commands.UpdateEmployeeProfile;

internal sealed class UpdateEmployeeProfileCommandValidator : AbstractValidator<UpdateEmployeeProfileCommand>
{
    private static readonly Regex PhoneRegex = new(@"^[0-9]{9,11}$", RegexOptions.Compiled);
    private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
    
    public UpdateEmployeeProfileCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithErrorCode("Employee.UserName.Required")
            .WithMessage("Tên nhân viên không được để trống")
            .MinimumLength(3)
            .WithErrorCode("Employee.UserName.TooShort")
            .WithMessage("Tên nhân viên phải có ít nhất 3 ký tự")
            .MaximumLength(50)
            .WithErrorCode("Employee.UserName.TooLong")
            .WithMessage("Tên nhân viên không được vượt quá 50 ký tự");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithErrorCode("Employee.PhoneNumber.Required")
            .WithMessage("Số điện thoại không được để trống")
            .Must(BeAValidPhoneNumber)
            .WithErrorCode("Employee.PhoneNumber.Invalid")
            .WithMessage("Số điện thoại không hợp lệ. Phải có từ 9-11 chữ số");

        RuleFor(x => x.DateOfBirth)
            .Must(BeAValidDateOfBirth).When(x => x.DateOfBirth.HasValue)
            .WithErrorCode("Employee.DateOfBirth.Invalid")
            .WithMessage("Ngày sinh không hợp lệ. Nhân viên phải từ 18 tuổi trở lên và không được vượt quá 100 tuổi");

        // RuleFor(x => x.ImageFile)
        //     .Must(BeAValidImageFile).When(x => x != null && x.ImageFile != null && x.ImageFile.Length > 0)
        //     .WithErrorCode("Employee.ImageFile.Invalid")
        //     .WithMessage($"Tệp ảnh không hợp lệ. Chỉ chấp nhận các định dạng: {string.Join(", ", AllowedImageExtensions)}. Kích thước tối đa: 5MB");
    }
    
    private static bool BeAValidPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return false;
            
        return PhoneRegex.IsMatch(phoneNumber);
    }
    
    private static bool BeAValidDateOfBirth(DateOnly? dateOfBirth)
    {
        if (!dateOfBirth.HasValue)
            return true;
            
        int age = DateTime.Today.Year - dateOfBirth.Value.Year;
        if (dateOfBirth.Value > DateOnly.FromDateTime(DateTime.Today.AddYears(-age)))
            age--;
            
        return age >= 18 && age <= 100;
    }
    
    // private static bool BeAValidImageFile(Microsoft.AspNetCore.Http.IFormFile file)
    // {
    //     if (file == null || file.Length == 0)
    //         return true;
            
    //     // Kiểm tra kích thước (tối đa 5MB)
    //     if (file.Length > 5 * 1024 * 1024)
    //         return false;
            
    //     // Kiểm tra phần mở rộng
    //     var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
    //     return AllowedImageExtensions.Contains(fileExtension);
    // }
} 