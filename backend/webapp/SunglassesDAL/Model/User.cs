using System;
using System.Collections.Generic;

namespace SunglassesDAL.Model;

public partial class User
{
    public Guid UserId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateOnly CreatedAt { get; set; }

    public DateOnly? UpdatedAt { get; set; }

    public DateOnly? ModifiedAt { get; set; }

    public bool IsAdmin { get; set; }

    public bool IsLogged { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
