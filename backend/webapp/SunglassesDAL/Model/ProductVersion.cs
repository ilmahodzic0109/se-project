using System;
using System.Collections.Generic;

namespace SunglassesDAL.Model;

public partial class ProductVersion
{
    public int VersionId { get; set; }

    public Guid ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string? Model { get; set; }

    public DateOnly? DatePublished { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public decimal Price { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }



    public virtual Product Product { get; set; } = null!;
}
