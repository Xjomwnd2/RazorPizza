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

        public async Task<bool> AddToCart(int pizzaId, List<int> toppingIds, int quantity = 1)
        {
            try
            {
                var pizza = await _pizzaService.GetPizzaByIdAsync(pizzaId);
                if (pizza == null)
                    return false;

                var toppings = await _toppingService.GetToppingsByIdsAsync(toppingIds);
                
                decimal itemPrice = pizza.Price;
                foreach (var topping in toppings)
                {
                    itemPrice += topping.Price;
                }

                var cart = await GetCart();

                var existingItem = cart.Items.FirstOrDefault(i => 
                    i.PizzaId == pizzaId && 
                    i.SelectedToppingIds.OrderBy(t => t).SequenceEqual(toppingIds.OrderBy(t => t)));

                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                    existingItem.TotalPrice = itemPrice * existingItem.Quantity;
                }
                else
                {
                    var newItem = new CartItem
                    {
                        PizzaId = pizzaId,
                        PizzaName = pizza.Name,
                        BasePrice = pizza.Price,
                        Quantity = quantity,
                        SelectedToppingIds = toppingIds,
                        TotalPrice = itemPrice * quantity
                    };
                    cart.Items.Add(newItem);
                }

                cart.Subtotal = cart.Items.Sum(i => i.TotalPrice);
                cart.Total = cart.Subtotal - cart.Discount;

                await SaveCart(cart);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoveFromCart(int pizzaId)
        {
            var cart = await GetCart();
            var itemToRemove = cart.Items.FirstOrDefault(i => i.PizzaId == pizzaId);
            
            if (itemToRemove == null)
                return false;

            cart.Items.Remove(itemToRemove);
            
            cart.Subtotal = cart.Items.Sum(i => i.TotalPrice);
            cart.Total = cart.Subtotal - cart.Discount;
            
            await SaveCart(cart);
            return true;
        }

        public async Task<bool> UpdateQuantity(int pizzaId, int newQuantity)
        {
            if (newQuantity <= 0)
                return await RemoveFromCart(pizzaId);

            var cart = await GetCart();
            var item = cart.Items.FirstOrDefault(i => i.PizzaId == pizzaId);
            
            if (item == null)
                return false;

            decimal pricePerItem = item.TotalPrice / item.Quantity;
            item.Quantity = newQuantity;
            item.TotalPrice = pricePerItem * newQuantity;
            
            cart.Subtotal = cart.Items.Sum(i => i.TotalPrice);
            cart.Total = cart.Subtotal - cart.Discount;
            
            await SaveCart(cart);
            return true;
        }

        public async Task<CartModel> GetCart()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null)
                return new CartModel();

            var cartJson = session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(cartJson))
            {
                return new CartModel();
            }

            return JsonSerializer.Deserialize<CartModel>(cartJson) ?? new CartModel();
        }

        public async Task<decimal> CalculateTotal(string? promoCode = null)
        {
            var cart = await GetCart();
            decimal total = cart.Subtotal;

            if (!string.IsNullOrEmpty(promoCode))
            {
                var discount = await _promoCodeService.ValidateAndCalculateDiscountAsync(promoCode, total);
                total -= discount;
                cart.Discount = discount;
            }

            return total;
        }

        public async Task<bool> ApplyPromoCode(string promoCode)
        {
            var cart = await GetCart();
            var discount = await _promoCodeService.ValidateAndCalculateDiscountAsync(promoCode, cart.Subtotal);
            
            if (discount > 0)
            {
                cart.PromoCode = promoCode;
                cart.Discount = discount;
                cart.Total = cart.Subtotal - discount;
                await SaveCart(cart);
                return true;
            }

            return false;
        }

        public async Task ClearCart()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                session.Remove(CartSessionKey);
            }
            await Task.CompletedTask;
        }

        private async Task SaveCart(CartModel cart)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                var cartJson = JsonSerializer.Serialize(cart);
                session.SetString(CartSessionKey, cartJson);
            }
            await Task.CompletedTask;
        }
    }
}