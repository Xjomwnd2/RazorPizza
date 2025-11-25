namespace RazorPizza.Models;

public class Order
{
    public int OrderId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal SubTotal { get; set; }
    public decimal Tax { get; set; }
    public decimal DeliveryFee { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Preparing, Baking, Delivering, Completed, Cancelled
    public string DeliveryAddress { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string? PromoCode { get; set; }
    public decimal Discount { get; set; }
    
    // Navigation properties
    public virtual ApplicationUser? User { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}