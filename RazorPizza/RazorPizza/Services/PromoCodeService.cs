using Microsoft.EntityFrameworkCore;
using RazorPizza.Data;
using RazorPizza.Models;

namespace RazorPizza.Services;

public class PromoCodeService : IPromoCodeService
{
    private readonly PizzaDbContext _context;

    public PromoCodeService(PizzaDbContext context)
    {
        _context = context;
    }

    public async Task<PromoCode?> ValidatePromoCodeAsync(string code, decimal? orderAmount = null)
    {
        var promo = await _context.PromoCodes
            .FirstOrDefaultAsync(p => p.Code == code && p.IsActive && p.ExpiryDate > DateTime.UtcNow);

        if (promo == null)
            return null;

        // Check minimum order amount if specified
        if (orderAmount.HasValue && promo.MinOrderAmount.HasValue && orderAmount.Value < promo.MinOrderAmount.Value)
            return null;

        return promo;
    }

    public decimal ApplyDiscount(decimal amount, PromoCode promoCode)
    {
        if (promoCode.DiscountType == "Percentage")
            return amount * (1 - promoCode.DiscountValue / 100);
        else
            return amount - promoCode.DiscountValue;
    }
}