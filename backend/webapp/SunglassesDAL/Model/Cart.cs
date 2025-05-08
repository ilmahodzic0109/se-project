using System;
using System.Collections.Generic;

namespace SunglassesDAL.Model;

public partial class Cart
{
    public int CartId { get; set; }

    public int Quantity { get; set; }

    public DateTime AddedAt { get; set; }

    public Guid UserId { get; set; }

    public Guid ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;
    public bool IsSelected { get; set; }
}
