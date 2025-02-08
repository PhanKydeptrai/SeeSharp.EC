using System;
using System.Collections.Generic;

namespace Persistence.Database.PostgreSQL.ReadModels;

public partial class OrderDetail
{
    public string OrderDetailId { get; set; } = null!;

    public string OrderId { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual OrderReadModel Order { get; set; } = null!;

    public virtual ProductReadModel Product { get; set; } = null!;
}
