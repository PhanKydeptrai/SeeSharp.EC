namespace Application.DTOs.Voucher;

public record VoucherResponse(
    Guid VoucherId, 
    string VoucherName, 
    string VoucherCode, 
    string VoucherType, 
    string PercentageDiscount, 
    string MaximumDiscountAmount, 
    string MinimumOrderAmount, 
    DateTime StartDate, 
    DateTime ExpiredDate, 
    string VoucherDescription, 
    string Status);
