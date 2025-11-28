using System.Collections.Generic;
using RazorPizza.Models;

namespace RazorPizza.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }   // Used in ShoppingCart.razor
        public int PizzaId { get; set; }
        public string PizzaName { get; set; }

        // Base price of the pizza
        public decimal Price { get; set; }

        // Quantity in the cart
        public int Quantity { get; set; } = 1;

        // List of topping IDs selected for this cart item
        public List<int> SelectedToppingIds { get; set; } = new();

        // Optional list of actual topping objects
        public List<Topping> SelectedToppings { get; set; } = new();

        // Computed total (pizza + toppings) × quantity
        public decimal TotalPrice
        {
            get
            {
                var toppingTotal = SelectedToppings?.Sum(t => t.Price) ?? 0m;
                return (Price + toppingTotal) * Quantity;
            }
        }
    }
}
