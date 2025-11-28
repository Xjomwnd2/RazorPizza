namespace RazorPizza.Models
{
    public class CartModel
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public string? PromoCode { get; set; }
    }
}