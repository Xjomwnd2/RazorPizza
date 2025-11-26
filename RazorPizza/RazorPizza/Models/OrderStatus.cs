// File: OrderStatus.cs
namespace RazorPizza.Models;

public static class OrderStatuses
{
    public const string Pending = "Pending";
    public const string Preparing = "Preparing";
    public const string Baking = "Baking";
    public const string QualityCheck = "Quality Check";
    public const string OutForDelivery = "Out for Delivery";
    public const string Delivered = "Delivered";
    public const string Completed = "Completed";
    public const string Cancelled = "Cancelled";
    
    public static List<string> AllStatuses = new()
    {
        Pending, Preparing, Baking, QualityCheck, OutForDelivery, Delivered, Completed, Cancelled
    };
} // <-- ADD THIS CLOSING BRACE