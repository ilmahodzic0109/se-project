namespace webapp.dto
{
    public class UpdateProductDto
    {
        public string Name { get; set; } // Product name
        public string Model { get; set; } // Product model
        public DateOnly DatePublished { get; set; } // Product published date
        public string Description { get; set; } // Product description
        public int DeliveryTime { get; set; } // Product delivery time
        public string Image { get; set; } // Product image URL
        public decimal Price { get; set; } // Product price
        public decimal ShippingPrice { get; set; } // Shipping price
        public int ColorId { get; set; } // Color ID (references Color table)
        public int ConditionId { get; set; } // Condition ID (references Condition table)
        public int Gender { get; set; } // Gender (references Gender table)
        public int ProductCategory { get; set; } // Product category (references ProductCategory table)
        public int BrandId { get; set; } // Brand ID (references Brand table)
        public int QuantityInStock { get; set; }  // New field for QuantityInStock
        public int ItemsSold { get; set; }
    }
}