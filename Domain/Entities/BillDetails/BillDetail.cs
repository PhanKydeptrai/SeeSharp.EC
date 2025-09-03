using Domain.Entities.Bills;
using Domain.Entities.Products;
using Domain.Entities.ProductVariants;

namespace Domain.Entities.BillDetails;

public sealed class BillDetail
{
    public BillDetailId BillDetailId { get; private set; } = null!;
    public BillId BillId { get; private set; } = null!;
    public ProductName ProductName { get; private set; } = ProductName.Empty;
    public VariantName VariantName { get; private set; } = VariantName.Empty;
    public ProductVariantPrice ProductVariantPrice { get; private set; } = null!;
    public BillDetailUnitPrice UnitPrice { get; private set; } = null!;
    public string ImageUrl { get; private set; } = string.Empty;
    public BillDetailQuantity BillDetailQuantity { get; private set; } = null!;
    public ColorCode ColorCode { get; private set; } = null!;
    public ProductVariantDescription ProductVariantDescription { get; private set; } = null!;

    // Foreign key
    public Bill Bill { get; set; } = null!;
    private BillDetail(
        BillDetailId billDetailId,
        BillId billId,
        ProductName productName,
        VariantName variantName,
        ProductVariantPrice productVariantPrice,
        BillDetailUnitPrice unitPrice,
        string imageUrl,
        BillDetailQuantity billDetailQuantity,
        ColorCode colorCode,
        ProductVariantDescription productVariantDescription)
    {
        BillDetailId = billDetailId;
        BillId = billId;
        ProductName = productName;
        VariantName = variantName;
        ProductVariantPrice = productVariantPrice;
        UnitPrice = unitPrice;
        ImageUrl = imageUrl;
        BillDetailQuantity = billDetailQuantity;
        ColorCode = colorCode;
        ProductVariantDescription = productVariantDescription;
    }

    public static BillDetail Create(
        BillId billId,
        ProductName productName,
        VariantName variantName,
        ProductVariantPrice productVariantPrice,
        BillDetailUnitPrice unitPrice,
        string imageUrl,
        BillDetailQuantity billDetailQuantity,
        ColorCode colorCode,
        ProductVariantDescription productVariantDescription)
    {
        return new BillDetail(
            BillDetailId.New(),
            billId,
            productName,
            variantName,
            productVariantPrice,
            unitPrice,
            imageUrl,
            billDetailQuantity,
            colorCode,
            productVariantDescription);
    }

    public void UpdateQuantity(BillDetailQuantity newQuantity)
    {
        if (newQuantity.Value <= BillDetailQuantity.Value)
        {
            throw new ArgumentException("Quantity must be greater than zero.", nameof(newQuantity));
        }

        if (newQuantity.Value < 1)
        {
            throw new ArgumentException("Quantity must be at least 1.", nameof(newQuantity));
        }

        BillDetailQuantity = newQuantity;
    }
}
