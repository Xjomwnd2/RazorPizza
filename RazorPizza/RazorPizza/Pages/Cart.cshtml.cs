using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPizza.Services;
using RazorPizza.Models;

namespace RazorPizza.Pages
{
    public class CartModel : PageModel
    {
        private readonly ICartService _cartService;
        private readonly IPromoCodeService _promoCodeService;

        public CartModel(ICartService cartService, IPromoCodeService promoCodeService)
        {
            _cartService = cartService;
            _promoCodeService = promoCodeService;
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

        public async Task<IActionResult> OnPostApplyPromo()
        {
            if (!string.IsNullOrEmpty(PromoCode))
            {
                var discount = await _promoCodeService.ValidateAndCalculateDiscountAsync(PromoCode, CartTotal);
                if (discount > 0)
                {
                    Message = $"Promo code applied! You saved ${discount:F2}";
                }
                else
                {
                    Message = "Invalid or expired promo code.";
                }
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
            // For now, just redirect to a confirmation page or clear cart
            return RedirectToPage("/OrderConfirmation");
        }

        private void LoadCart()
        {
            CartItems = _cartService.GetCart();
            CartTotal = _cartService.GetCartTotal();
        }
    }
}