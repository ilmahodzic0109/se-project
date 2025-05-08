using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SunglassesDAL.Model;

public partial class Product
{
    public Guid ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string? Model { get; set; }

    public DateOnly? DatePublished { get; set; }

    public string? Description { get; set; }

    public int DeliveryTime { get; set; }

    public string? Image { get; set; }

    public decimal Price { get; set; }

    public decimal ShippingPrice { get; set; }

    public bool IsDeleted { get; set; }

    public int ColorId { get; set; }

    public int ConditionId { get; set; }

    public int Gender { get; set; }

    public int ProductCategory { get; set; }

    public int BrandId { get; set; }
    [JsonIgnore]
    public virtual Brand Brand { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
    [JsonIgnore]
    public virtual Color Color { get; set; } = null!;
    [JsonIgnore]
    public virtual Condition Condition { get; set; } = null!;
    [JsonIgnore]
    public virtual Gender GenderNavigation { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    [JsonIgnore]
    public virtual ProductCategory ProductCategoryNavigation { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<ProductInventory> ProductInventories { get; set; } = new List<ProductInventory>();

    public virtual ICollection<ProductVersion> ProductVersions { get; set; } = new List<ProductVersion>();
    public int VersionId { get; set; }  
    public bool IsCurrent { get; set; }
}
