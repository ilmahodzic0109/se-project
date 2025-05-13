using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webapp.dto
{
    public class ProductDetailsDto
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public DateOnly? DatePublished { get; set; }
        public string Description { get; set; }
        public int DeliveryTime { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public decimal ShippingPrice { get; set; }
        public string Color { get; set; }
        public string Condition { get; set; }
        public string Gender { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public int QuantityInStock { get; set; }
        public int ItemsSold { get; set; }
        public bool IsDeleted { get; set; }
    }
}