namespace MyProject.Application.DTO
{
    public class CartDTO
    {
        public IEnumerable<CartItemDTO> Items { get; set; }
        public double TotalPrice { get; set; } 
    }

    public class CartItemDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; } 
        public double TotalPrice => Price * Quantity;
    }
}
