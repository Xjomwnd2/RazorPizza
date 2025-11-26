using RazorPizza.Models;

namespace RazorPizza.Services;

public class CartService : ICartService
{
    private List<CartItem> _cart = new();

    public List<CartItem> GetCartItems() => _cart;

    public void AddToCart(CartItem item)
    {
        var existing = _cart.FirstOrDefault(c => c.PizzaId == item.PizzaId);
        if (existing != null)
            existing.Quantity += item.Quantity;
        else
            _cart.Add(item);
    }

    public void RemoveFromCart(int pizzaId)
    {
        _cart.RemoveAll(c => c.PizzaId == pizzaId);
    }

    public void UpdateQuantity(int pizzaId, int quantity)
    {
        var item = _cart.FirstOrDefault(c => c.PizzaId == pizzaId);
        if (item != null)
            item.Quantity = quantity;
    }

    public void ClearCart() => _cart.Clear();

    public decimal GetCartTotal() => _cart.Sum(c => c.TotalPrice);
}