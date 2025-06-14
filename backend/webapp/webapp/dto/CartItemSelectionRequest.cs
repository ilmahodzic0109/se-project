namespace webapp.dto
{
    public class CartItemSelectionRequest
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public bool IsSelected { get; set; }
    }

}
