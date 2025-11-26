using RazorPizza.Models;

namespace RazorPizza.Services;

public interface IPromoCodeService
{
    Task<PromoCode?> ValidatePromoCodeAsync(string code);
    decimal ApplyDiscount(decimal amount, PromoCode promoCode);
}