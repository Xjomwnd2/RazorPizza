// File: Topping.cs
namespace RazorPizza.Models;

public class Topping
{
    public int ToppingId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = "Other"; // Meat, Vegetable, Cheese, Sauce, Other
    public bool IsAvailable { get; set; } = true;
    public string ImageUrl { get; set; } = "/images/toppings/default.jpg";
}