using System;
using System.Collections.Generic;

namespace SunglassesDAL.Model;

public partial class Color
{
    public int ColorId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
