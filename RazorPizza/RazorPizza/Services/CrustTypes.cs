// File: CrustTypes.cs
namespace RazorPizza.Models;

public static class CrustTypes
{
    public static Dictionary<string, decimal> CrustPrices = new()
    {
        { "Thin", 0.0m },
        { "Regular", 0.0m },
        { "Thick", 0.0m },
        { "Stuffed", 2.0m }
    };
}