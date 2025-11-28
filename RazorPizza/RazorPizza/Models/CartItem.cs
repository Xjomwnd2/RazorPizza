using System.Collections.Generic;
using System.Linq;

namespace RazorPizza.Models
{
    public class CartItem
    {
        public string Size { get; set; } = string.Empty;
        public string CrustType { get; set; } = string.Empty;
        public string? SpecialInstructions { get; set; }

        public int CartItemId { get; set; }
        public int PizzaId { get; set; }
        public string PizzaName { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public int Quantity { get; set; } = 1;

        // Sauce selection
        public string Sauce { get; set; } = string.Empty;

        // Topping IDs used in the UI
        public List<int> ToppingIds { get; set; } = new();

        // Toppings used by services
        public List<int> SelectedToppingIds { get; set; } = new();
        public List<Topping> SelectedToppings { get; set; } = new();

        // Auto-calculated total
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
