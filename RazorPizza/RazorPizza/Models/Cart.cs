namespace RazorPizza.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public List<CartItemEntity> Items { get; set; } = new List<CartItemEntity>();
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public string? PromoCode { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class CartItemEntity
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int PizzaId { get; set; }
        public string PizzaName { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public int Quantity { get; set; }
        public string ToppingsJson { get; set; } = "[]";
        public decimal TotalPrice { get; set; }
    }
}