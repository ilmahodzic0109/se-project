using System;
using System.Collections.Generic;

namespace SunglassesDAL.Model;

public partial class Payment
{
    public int PaymentId { get; set; }

    public Guid OrderId { get; set; }

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    public virtual Order Order { get; set; } = null!;
}
