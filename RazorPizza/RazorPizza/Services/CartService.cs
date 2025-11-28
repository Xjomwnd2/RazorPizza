using Microsoft.EntityFrameworkCore;
using RazorPizza.Data;
using RazorPizza.Models;
using System.Text.Json;

namespace RazorPizza.Services
{
    public class CartService : ICartService
    {
        private readonly IPizzaService _pizzaService;
        private readonly IToppingService _toppingService;
        private readonly IPromoCodeService _promoCodeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly PizzaDbContext _context;
        
        private const string CartSessionKey = "ShoppingCart";

        public CartService(
            IPizzaService pizzaService,
            IToppingService toppingService,
            IPromoCodeService promoCodeService,
            IHttpContextAccessor httpContextAccessor,
            PizzaDbContext context)
        {
            _pizzaService = pizzaService;
            _toppingService = toppingService;
            _promoCodeService = promoCodeService;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        // Get the cart as a List<CartItem>
        public List<CartItem> GetCart()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null)
                return new List<CartItem>();

            var cartJson = session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<CartItem>();
            }

            var cartModel = JsonSerializer.Deserialize<CartModel>(cartJson);
            return cartModel?.Items ?? new List<CartItem>();
        }

        // Get cart items (same as GetCart for now)
        public List<CartItem> GetCartItems()
        {
            return GetCart();
        }

        // Add item to cart
        public void AddToCart(CartItem item)
        {
            if (item == null)
                return;

            var cartItems = GetCart();

            // Check if item already exists (same pizza + same toppings)
            var existingItem = cartItems.FirstOrDefault(i => 
                i.PizzaId == item.PizzaId && 
                i.SelectedToppingIds.OrderBy(t => t).SequenceEqual(item.SelectedToppingIds.OrderBy(t => t)));

            if (existingItem != null)
            {
                // Update quantity of existing item
                existingItem.Quantity += item.Quantity;
                decimal pricePerItem = existingItem.TotalPrice / (existingItem.Quantity - item.Quantity);
                existingItem.TotalPrice = pricePerItem * existingItem.Quantity;
            }
            else
            {
                // Add new item
                cartItems.Add(item);
            }

            SaveCart(cartItems);
        }

        // Remove item from cart
        public void RemoveFromCart(int pizzaId)
        {
            var cartItems = GetCart();
            var itemToRemove = cartItems.FirstOrDefault(i => i.PizzaId == pizzaId);
            
            if (itemToRemove != null)
            {
                cartItems.Remove(itemToRemove);
                SaveCart(cartItems);
            }
        }

        // Update quantity of an item
        public void UpdateQuantity(int pizzaId, int newQuantity)
        {
            if (newQuantity <= 0)
            {
                RemoveFromCart(pizzaId);
                return;
            }

            var cartItems = GetCart();
            var item = cartItems.FirstOrDefault(i => i.PizzaId == pizzaId);
            
            if (item != null)
            {
                decimal pricePerItem = item.TotalPrice / item.Quantity;
                item.Quantity = newQuantity;
                SaveCart(cartItems);
            }
        }

        // Clear all items from cart
        public void ClearCart()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                session.Remove(CartSessionKey);
            }
        }

        // Get total price of all items in cart
        public decimal GetCartTotal()
        {
            var cartItems = GetCart();
            return cartItems.Sum(i => i.TotalPrice);
        }

        // Helper method to save cart to session
        private void SaveCart(List<CartItem> items)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                var cartModel = new CartModel
                {
                    Items = items,
                    Subtotal = items.Sum(i => i.TotalPrice),
                    Total = items.Sum(i => i.TotalPrice)
                };
                
                var cartJson = JsonSerializer.Serialize(cartModel);
                session.SetString(CartSessionKey, cartJson);
            }
        }
    }
}