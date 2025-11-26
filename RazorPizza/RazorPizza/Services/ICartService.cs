using RazorPizza.Models;

namespace RazorPizza.Services;
public interface ICartService
{
    List<CartItem> GetCartItems();
    List<CartItem> GetCart(); // Add this alias method
    void AddToCart(CartItem item);
    void RemoveFromCart(int pizzaId);
    void UpdateQuantity(int pizzaId, int quantity);
    void ClearCart();
    decimal GetCartTotal();
}