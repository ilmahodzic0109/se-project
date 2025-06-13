namespace webapp.dto
{
    public class AddProductDto
    {
        public string Name { get; set; }
        public string? Model { get; set; }
        public string? Description { get; set; }
        public int DeliveryTime { get; set; }
        public string? Image { get; set; }
        public decimal Price { get; set; }
        public decimal ShippingPrice { get; set; }
        public int ColorId { get; set; }
        public int ConditionId { get; set; }
        public int Gender { get; set; }
        public int ProductCategory { get; set; }
        public int BrandId { get; set; }
        public int QuantityInStock { get; set; }
        public int ItemsSold { get; set; }
    }
}