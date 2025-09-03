using System;

namespace Domain.Database.PostgreSQL.ReadModels;

public class BillDetailReadModel
{
    public Ulid BillDetailId { get; set; }
    public Ulid BillId { get; set; }
    public string ProductName { get; set; } = null!;
    public string VariantName { get; set; } = null!;
    public decimal ProductVariantPrice { get; set; }
    public decimal UnitPrice { get; set; }
    public string ImageUrl { get; set; } = null!;
    public int BillDetailQuantity { get; set; }
    public string ColorCode { get; set; } = null!;
    public string ProductVariantDescription { get; set; } = null!;
    public BillReadModel Bill { get; set; } = null!; 
}
