namespace webapp.dto
{
    public class CartItemDto
    {
        public int CartId { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedAt { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public string ColorName { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal Subtotal { get; set; }
    }
}
