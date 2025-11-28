using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPizza.Services;
using RazorPizza.Models;

namespace RazorPizza.Pages
{
    public class CartModel : PageModel
    {
        private readonly ICartService _cartService;

        public CartModel(ICartService cartService)
        {
            _cartService = cartService;
        }

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public decimal CartTotal { get; set; }

        [BindProperty]
        public string? PromoCode { get; set; }

        public string? Message { get; set; }

        public void OnGet()
        {
            LoadCart();
        }

        public IActionResult OnPostRemoveItem(int pizzaId)
        {
            _cartService.RemoveFromCart(pizzaId);
            return RedirectToPage();
        }

        public IActionResult OnPostUpdateQuantity(int pizzaId, int quantity)
        {
            _cartService.UpdateQuantity(pizzaId, quantity);
            return RedirectToPage();
        }

        public IActionResult OnPostApplyPromo()
        {
            if (!string.IsNullOrEmpty(PromoCode))
            {
                // TODO: Implement promo code validation
                Message = "Promo code feature coming soon!";
            }
            LoadCart();
            return Page();
        }

        public IActionResult OnPostCheckout()
        {
            if (!CartItems.Any())
            {
                Message = "Your cart is empty!";
                return Page();
            }

            // TODO: Implement checkout logic
            // For now, just clear the cart
            _cartService.ClearCart();
            return RedirectToPage("/Index");
        }

        private void LoadCart()
        {
            CartItems = _cartService.GetCart();
            CartTotal = _cartService.GetCartTotal();
        }
    }
}