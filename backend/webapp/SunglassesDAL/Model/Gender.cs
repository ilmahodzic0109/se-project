using System;
using System.Collections.Generic;

namespace SunglassesDAL.Model;

public partial class Gender
{
    public int GenderId { get; set; }

    public bool Name { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
