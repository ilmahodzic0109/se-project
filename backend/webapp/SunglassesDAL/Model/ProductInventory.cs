using System;
using System.Collections.Generic;

namespace SunglassesDAL.Model;

public partial class ProductInventory
{
    public int ProductInventoryId { get; set; }

    public int QuantityInStock { get; set; }

    public int ItemSold { get; set; }

    public Guid ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;
}
