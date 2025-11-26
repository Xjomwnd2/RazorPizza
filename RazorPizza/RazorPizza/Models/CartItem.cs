namespace RazorPizza.Models;

public class CartItem
{
    public int CartItemId { get; set; }
    public int? PizzaId { get; set; }
    public string PizzaName { get; set; } = string.Empty;
    public string Size { get; set; } = "Medium";
    public string CrustType { get; set; } = "Regular";
    public string Sauce { get; set; } = "Tomato";
    public List<int> ToppingIds { get; set; } = new();
    public int Quantity { get; set; } = 1;
    public decimal Price { get; set; }
    public decimal TotalPrice => Price * Quantity;
    public string? SpecialInstructions { get; set; }
}