using Application.Abstractions.LinkService;

namespace Application.DTOs.Product;

/// <summary>
/// ProductResponse lấy thông tin cho chế độ xem tổng quan
/// </summary>
/// <param name="ProductId"></param>
/// <param name="ProductName"></param>
/// <param name="PriceOfBaseVariant"></param>
/// <param name="ImageUrl"></param>
/// <param name="Description"></param>
/// <param name="Status"></param>
/// <param name="CategoryName"></param>
/// <param name="Variants"></param>
public record ProductResponse(
    Guid ProductId,
    string ProductName,
    decimal PriceOfBaseVariant,
    string? ImageUrl,
    string? Description,
    string Status,
    string CategoryName,
    VariantResponse[] Variants)
{
    public List<Link> links { get; set; } = new();
}

/// <summary>
/// Hỗ trợ bổ sung thông tin
/// </summary>
/// <param name="ProductVariantId"></param>
/// <param name="ProductId"></param>
/// <param name="VariantName"></param>
/// <param name="ColorCode"></param>
/// <param name="Description"></param>
/// <param name="Price"></param>
/// <param name="ImageUrl"></param>
/// <param name="IsBaseVariant"></param>
public record VariantResponse(
    Guid ProductVariantId,
    Guid ProductId,
    string VariantName,
    string ColorCode,
    string Description,
    decimal Price,
    string? ImageUrl,
    bool IsBaseVariant);
