namespace Application.DTOs.Product;
/// <summary>
/// Lấy thông tin độc lập
/// </summary>
/// <param name="ProductVariantId"></param>
/// <param name="ProductId"></param>
/// <param name="ProductName"></param>
/// <param name="VariantName"></param>
/// <param name="ColorCode"></param>
/// <param name="Description"></param>
/// <param name="Price"></param>
/// <param name="ImageUrl"></param>
/// <param name="IsBaseVariant"></param>
public record ProductVariantResponse(
    Guid ProductVariantId,
    Guid ProductId,
    string ProductName,
    string VariantName,
    string ColorCode,
    string Description,
    string CategoryName,
    decimal Price,
    string? ImageUrl,
    bool IsBaseVariant);