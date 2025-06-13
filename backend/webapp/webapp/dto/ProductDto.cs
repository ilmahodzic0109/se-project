namespace webapp.dto
{
    public class ProductDTO
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public string Condition { get; set; }
        public string Category { get; set; }
        public string brandName { get; set; }
    }

}