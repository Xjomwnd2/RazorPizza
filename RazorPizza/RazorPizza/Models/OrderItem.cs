namespace RazorPizza.Models;

public class OrderItem
{
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public int? PizzaId { get; set; }
    public int Quantity { get; set; } = 1;
    public string Size { get; set; } = "Medium"; // Small, Medium, Large, ExtraLarge
    public string CrustType { get; set; } = "Regular"; // Thin, Regular, Thick, Stuffed
    public string Sauce { get; set; } = "Tomato"; // Tomato, White, BBQ, Pesto, None
    public string? CustomToppings { get; set; } // JSON string of topping IDs
    public decimal Price { get; set; }
    public string? SpecialInstructions { get; set; }
    
    // Navigation properties
    public virtual Order? Order { get; set; }
    public virtual Pizza? Pizza { get; set; }
}