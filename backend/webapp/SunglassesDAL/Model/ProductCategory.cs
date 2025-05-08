using System;
using System.Collections.Generic;

namespace SunglassesDAL.Model;

public partial class ProductCategory
{
    public int ProductCategoryId { get; set; }

    public bool Category { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
