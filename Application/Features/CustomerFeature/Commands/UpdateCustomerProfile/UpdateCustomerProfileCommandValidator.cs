using FluentValidation;
using System;
using System.Text.RegularExpressions;
using System.IO;
using Domain.Entities.Users;

namespace Application.Features.CustomerFeature.Commands.UpdateCustomerProfile;

internal sealed class UpdateCustomerProfileCommandValidator : AbstractValidator<UpdateCustomerProfileCommand>
{
    private static readonly Regex PhoneRegex = new(@"^[0-9]{9,11}$", RegexOptions.Compiled);
    private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png" };
    private const int MaxImageSizeMB = 5;
    
    public UpdateCustomerProfileCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithErrorCode("Customer.UserId.Required")
            .WithMessage("User ID không được để trống");
            
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithErrorCode("Customer.UserName.Required")
            .WithMessage("Tên khách hàng không được để trống")
            .MinimumLength(3)
            .WithErrorCode("Customer.UserName.TooShort")
            .WithMessage("Tên khách hàng phải có ít nhất 3 ký tự")
            .MaximumLength(50)
            .WithErrorCode("Customer.UserName.TooLong")
            .WithMessage("Tên khách hàng không được vượt quá 50 ký tự");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithErrorCode("Customer.PhoneNumber.Required")
            .WithMessage("Số điện thoại không được để trống")
            .Must(BeAValidPhoneNumber)
            .WithErrorCode("Customer.PhoneNumber.Invalid")
            .WithMessage("Số điện thoại không hợp lệ. Phải có từ 9-11 chữ số");

        RuleFor(x => x.Gender)
            .Must(gender => Enum.IsDefined(typeof(Gender), gender))
            .WithErrorCode("Customer.Gender.Invalid")
            .WithMessage("Giới tính không hợp lệ");

        RuleFor(x => x.DateOfBirth)
            .Must(BeAValidDateOfBirth).When(x => x.DateOfBirth.HasValue)
            .WithErrorCode("Customer.DateOfBirth.Invalid")
            .WithMessage("Ngày sinh không hợp lệ. Khách hàng phải từ 13 tuổi trở lên và không được vượt quá 120 tuổi");

        RuleFor(x => x.ImageFile)
            .Must(BeAValidImageFile).When(x => x?.ImageFile != null && x.ImageFile.Length > 0)
            .WithErrorCode("Customer.ImageFile.Invalid")
            .WithMessage($"Tệp ảnh không hợp lệ. Chỉ chấp nhận các định dạng: {string.Join(", ", AllowedImageExtensions)}. Kích thước tối đa: {MaxImageSizeMB}MB");
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
            
        return age >= 13 && age <= 120;
    }
    
    private static bool BeAValidImageFile(Microsoft.AspNetCore.Http.IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return true;
            
        // Check size (max 5MB)
        if (file.Length > MaxImageSizeMB * 1024 * 1024)
            return false;
            
        // Check file extension
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return AllowedImageExtensions.Contains(fileExtension);
    }
} 