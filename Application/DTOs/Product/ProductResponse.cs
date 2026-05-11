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
    Guid BaseVariantId,
    string ProductName,
    decimal PriceOfBaseVariant,
    string? ImageUrl,
    string? Description,
    string Status,
    string CategoryName,
    ProductVariantResponse[] Variants);
// {
//     public List<Link> links { get; set; } = new();
// }

