// File: Pizza.cs
namespace RazorPizza.Models;

public class Pizza
{
    public int PizzaId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public string ImageUrl { get; set; } = "/images/default-pizza.jpg";
    public string Category { get; set; } = "Custom"; // Specialty, Custom, Vegetarian, etc.
    public bool IsAvailable { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
