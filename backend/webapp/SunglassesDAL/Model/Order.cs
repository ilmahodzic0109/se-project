using System;
using System.Collections.Generic;

namespace SunglassesDAL.Model;

public partial class Order
{
    public Guid OrderId { get; set; }

    public DateTime Date { get; set; }

    public decimal TotalPrice { get; set; }

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Country { get; set; } = null!;

    public long PhoneNumber { get; set; }

    public int PostalCode { get; set; }

    public Guid UserId { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual User User { get; set; } = null!;
}
