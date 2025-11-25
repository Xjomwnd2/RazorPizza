namespace RazorPizza.Models;

public class PromoCode
{
    public int PromoCodeId { get; set; }
    public string Code { get; set; } = string.Empty;
    public decimal DiscountPercent { get; set; }
    public decimal? MaxDiscountAmount { get; set; }
    public decimal? MinOrderAmount { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime ExpiryDate { get; set; }
    public bool IsActive { get; set; } = true;
    public int? UsageLimit { get; set; }
    public int UsageCount { get; set; } = 0;
}