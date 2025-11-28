namespace RazorPizza.Models;
public class CartItem
{
    public int PizzaId { get; set; }
    public string PizzaName { get; set; }
    public decimal BasePrice { get; set; }

    // ADD THIS
    public List<int> SelectedToppingIds { get; set; } = new List<int>();

    public decimal TotalPrice => BasePrice + (SelectedToppings?.Sum(t => t.Price) ?? 0);

    public List<Topping> SelectedToppings { get; set; } = new();
}
