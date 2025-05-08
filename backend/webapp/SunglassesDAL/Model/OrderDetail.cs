using System;
using System.Collections.Generic;

namespace SunglassesDAL.Model;

public partial class OrderDetail
{
    public int OrderDetailsId { get; set; }

    public int Quantity { get; set; }

    public decimal PriceAtPurchase { get; set; }

    public decimal Subtotal { get; set; }

    public Guid OrderId { get; set; }

    public Guid ProductId { get; set; }

    

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    
}
